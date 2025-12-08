using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VedicAPI.API.Models;
using VedicAPI.API.Models.DTOs.Auth;
using VedicAPI.API.Repositories.Interfaces;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Services;

/// <summary>
/// Service implementation for authentication operations
/// </summary>
public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IAuthRepository authRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<AuthResponseDto> SignupAsync(SignupRequestDto signupDto)
    {
        try
        {
            // Check if email already exists
            var existingUser = await _authRepository.GetUserByEmailAsync(signupDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Hash password
            var passwordHash = HashPassword(signupDto.Password);

            // Create user
            var user = new User
            {
                Name = signupDto.Name,
                Email = signupDto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var createdUser = await _authRepository.CreateUserAsync(user);

            // Generate tokens
            var accessToken = GenerateAccessToken(createdUser);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // Save refresh token
            await _authRepository.UpdateRefreshTokenAsync(
                createdUser.Id,
                refreshToken,
                refreshTokenExpiry
            );

            // Update last login
            await _authRepository.UpdateLastLoginAsync(createdUser.Id);

            return new AuthResponseDto
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiry = DateTime.UtcNow.AddHours(24),
                CreatedAt = createdUser.CreatedAt,
                ProfileImageUrl = createdUser.ProfileImageUrl
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during signup for email: {Email}", signupDto.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto)
    {
        try
        {
            // Get user by email
            var user = await _authRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Verify password
            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Account is inactive");
            }

            // Generate tokens
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // Save refresh token
            await _authRepository.UpdateRefreshTokenAsync(
                user.Id,
                refreshToken,
                refreshTokenExpiry
            );

            // Update last login
            await _authRepository.UpdateLastLoginAsync(user.Id);

            return new AuthResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = accessToken,
                RefreshToken = refreshToken,
                TokenExpiry = DateTime.UtcNow.AddHours(24),
                CreatedAt = user.CreatedAt,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", loginDto.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenDto)
    {
        try
        {
            // Validate access token (without checking expiry)
            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
            if (principal == null)
            {
                throw new UnauthorizedAccessException("Invalid access token");
            }

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid token claims");
            }

            // Get user by refresh token
            var user = await _authRepository.GetUserByRefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (user == null || user.Id != userId)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            // Generate new tokens
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            // Update refresh token
            await _authRepository.UpdateRefreshTokenAsync(
                user.Id,
                newRefreshToken,
                refreshTokenExpiry
            );

            return new AuthResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                TokenExpiry = DateTime.UtcNow.AddHours(24),
                CreatedAt = user.CreatedAt,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> RevokeTokenAsync(int userId)
    {
        try
        {
            await _authRepository.RevokeRefreshTokenAsync(userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking token for user: {UserId}", userId);
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
    {
        try
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                ProfileImageUrl = user.ProfileImageUrl
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile for user: {UserId}", userId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> IsEmailAvailableAsync(string email)
    {
        var exists = await _authRepository.EmailExistsAsync(email);
        return !exists;
    }

    #region Private Helper Methods

    private string GenerateAccessToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "VedicAPI";
        var audience = jwtSettings["Audience"] ?? "VedicAI";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = false // Don't validate expiry for refresh
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }

    private string HashPassword(string password)
    {
        // Using BCrypt for password hashing
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    #endregion
}


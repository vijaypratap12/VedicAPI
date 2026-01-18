using VedicAPI.API.Models.DTOs.Auth;

namespace VedicAPI.API.Services.Interfaces;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Register a new user
    /// </summary>
    Task<AuthResponseDto> SignupAsync(SignupRequestDto signupDto);
    
    /// <summary>
    /// Authenticate user and generate tokens
    /// </summary>
    Task<AuthResponseDto> LoginAsync(LoginRequestDto loginDto);
    
    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenDto);
    
    /// <summary>
    /// Revoke user's refresh token (logout)
    /// </summary>
    Task<bool> RevokeTokenAsync(int userId);
    
    /// <summary>
    /// Get user profile by ID
    /// </summary>
    Task<UserProfileDto?> GetUserProfileAsync(int userId);
    
    /// <summary>
    /// Verify if email is available
    /// </summary>
    Task<bool> IsEmailAvailableAsync(string email);
    
    /// <summary>
    /// Request password reset OTP
    /// </summary>
    Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);
    
    /// <summary>
    /// Verify OTP for password reset
    /// </summary>
    Task<bool> VerifyOTPAsync(VerifyOTPRequestDto request);
    
    /// <summary>
    /// Reset password using OTP
    /// </summary>
    Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request);
}


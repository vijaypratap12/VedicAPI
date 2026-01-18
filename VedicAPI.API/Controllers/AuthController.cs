using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VedicAPI.API.Models.Common;
using VedicAPI.API.Models.DTOs.Auth;
using VedicAPI.API.Services.Interfaces;

namespace VedicAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="signupDto">Signup request data</param>
    /// <returns>Authentication response with tokens</returns>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Signup([FromBody] SignupRequestDto signupDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));
            }

            var result = await _authService.SignupAsync(signupDto);
            
            return CreatedAtAction(
                nameof(GetProfile),
                new { id = result.Id },
                ApiResponse<AuthResponseDto>.SuccessResponse(result, "User registered successfully")
            );
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Signup failed for email: {Email}", signupDto.Email);
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during signup for email: {Email}", signupDto.Email);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during registration"));
        }
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="loginDto">Login request data</param>
    /// <returns>Authentication response with tokens</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));
            }

            var result = await _authService.LoginAsync(loginDto);
            
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Login successful"));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Login failed for email: {Email}", loginDto.Email);
            return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", loginDto.Email);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during login"));
        }
    }

    /// <summary>
    /// Refresh access token
    /// </summary>
    /// <param name="refreshTokenDto">Refresh token request data</param>
    /// <returns>New authentication tokens</returns>
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));
            }

            var result = await _authService.RefreshTokenAsync(refreshTokenDto);
            
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Token refreshed successfully"));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Token refresh failed");
            return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during token refresh"));
        }
    }

    /// <summary>
    /// Logout user (revoke refresh token)
    /// </summary>
    /// <returns>Success response</returns>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> Logout()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid user"));
            }

            var result = await _authService.RevokeTokenAsync(userId);
            
            if (result)
            {
                return Ok(ApiResponse<object>.SuccessResponse(new object(), "Logout successful"));
            }
            
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Failed to logout"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred during logout"));
        }
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    /// <returns>User profile data</returns>
    [HttpGet("profile")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetProfile()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid user"));
            }

            var profile = await _authService.GetUserProfileAsync(userId);
            
            if (profile == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("User not found"));
            }
            
            return Ok(ApiResponse<UserProfileDto>.SuccessResponse(profile, "Profile retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving profile"));
        }
    }

    /// <summary>
    /// Check if email is available
    /// </summary>
    /// <param name="email">Email to check</param>
    /// <returns>Availability status</returns>
    [HttpGet("check-email")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<bool>>> CheckEmailAvailability([FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Email is required"));
            }

            var isAvailable = await _authService.IsEmailAvailableAsync(email);
            
            return Ok(ApiResponse<bool>.SuccessResponse(
                isAvailable,
                isAvailable ? "Email is available" : "Email is already taken"
            ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking email availability");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while checking email"));
        }
    }

    /// <summary>
    /// Request password reset OTP
    /// </summary>
    /// <param name="request">Forgot password request data</param>
    /// <returns>OTP response</returns>
    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(ApiResponse<ForgotPasswordResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ForgotPasswordResponseDto>>> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));
            }

            var result = await _authService.ForgotPasswordAsync(request);
            
            return Ok(ApiResponse<ForgotPasswordResponseDto>.SuccessResponse(result, "OTP generated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Forgot password failed for email: {Email}", request.Email);
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password for email: {Email}", request.Email);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while generating OTP"));
        }
    }

    /// <summary>
    /// Verify OTP for password reset
    /// </summary>
    /// <param name="request">OTP verification request data</param>
    /// <returns>Verification result</returns>
    [HttpPost("verify-otp")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> VerifyOTP([FromBody] VerifyOTPRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));
            }

            var isValid = await _authService.VerifyOTPAsync(request);
            
            if (!isValid)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid or expired OTP"));
            }
            
            return Ok(ApiResponse<object>.SuccessResponse(new object(), "OTP verified successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during OTP verification for email: {Email}", request.Email);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while verifying OTP"));
        }
    }

    /// <summary>
    /// Reset password using OTP
    /// </summary>
    /// <param name="request">Password reset request data</param>
    /// <returns>Reset result</returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));
            }

            var success = await _authService.ResetPasswordAsync(request);
            
            if (!success)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Invalid or expired OTP"));
            }
            
            return Ok(ApiResponse<object>.SuccessResponse(new object(), "Password reset successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Password reset failed for email: {Email}", request.Email);
            return Unauthorized(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for email: {Email}", request.Email);
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while resetting password"));
        }
    }
}


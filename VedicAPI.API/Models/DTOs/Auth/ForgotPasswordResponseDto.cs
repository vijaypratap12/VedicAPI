namespace VedicAPI.API.Models.DTOs.Auth;

/// <summary>
/// DTO for forgot password response containing OTP
/// </summary>
public class ForgotPasswordResponseDto
{
    /// <summary>
    /// Generated OTP code (6 digits)
    /// </summary>
    public string Otp { get; set; } = string.Empty;
    
    /// <summary>
    /// OTP expiry timestamp
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Success message
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

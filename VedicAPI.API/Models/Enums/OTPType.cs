namespace VedicAPI.API.Models.Enums;

/// <summary>
/// OTP Type constants for different OTP purposes
/// </summary>
public static class OTPType
{
    /// <summary>
    /// OTP for password reset
    /// </summary>
    public const string PasswordReset = "PasswordReset";
    
    /// <summary>
    /// OTP for user signup verification
    /// </summary>
    public const string Signup = "Signup";
    
    /// <summary>
    /// OTP for email verification
    /// </summary>
    public const string EmailVerification = "EmailVerification";
    
    /// <summary>
    /// OTP for two-factor authentication
    /// </summary>
    public const string TwoFactorAuth = "TwoFactorAuth";
}

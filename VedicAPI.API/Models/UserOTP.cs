namespace VedicAPI.API.Models;

/// <summary>
/// User OTP entity representing OTP records for various purposes
/// Generic table for PasswordReset, Signup, EmailVerification, etc.
/// </summary>
public class UserOTP
{
    public int Id { get; set; }
    
    /// <summary>
    /// User ID (nullable for signup OTP before user creation)
    /// </summary>
    public int? UserId { get; set; }
    
    /// <summary>
    /// Email address (required)
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// OTP code (6 digits)
    /// </summary>
    public string OTP { get; set; } = string.Empty;
    
    /// <summary>
    /// OTP type: PasswordReset, Signup, EmailVerification, etc.
    /// </summary>
    public string OTPType { get; set; } = string.Empty;
    
    /// <summary>
    /// OTP expiry timestamp
    /// </summary>
    public DateTime OTPExpiry { get; set; }
    
    /// <summary>
    /// Whether OTP has been used
    /// </summary>
    public bool IsUsed { get; set; }
    
    /// <summary>
    /// When OTP was requested
    /// </summary>
    public DateTime RequestedAt { get; set; }
    
    /// <summary>
    /// Record creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Record last update timestamp
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Navigation property to User (optional, nullable)
    /// </summary>
    public User? User { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs.Auth;

/// <summary>
/// DTO for password reset request
/// OTP is verified again during password reset
/// </summary>
public class ResetPasswordRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "OTP is required")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be exactly 6 digits")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must contain only digits")]
    public string Otp { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    public string NewPassword { get; set; } = string.Empty;
}

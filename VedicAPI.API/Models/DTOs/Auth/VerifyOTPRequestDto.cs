using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs.Auth;

/// <summary>
/// DTO for OTP verification request
/// </summary>
public class VerifyOTPRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "OTP is required")]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be exactly 6 digits")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must contain only digits")]
    public string Otp { get; set; } = string.Empty;
}

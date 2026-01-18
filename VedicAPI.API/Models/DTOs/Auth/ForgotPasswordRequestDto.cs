using System.ComponentModel.DataAnnotations;

namespace VedicAPI.API.Models.DTOs.Auth;

/// <summary>
/// DTO for forgot password request
/// </summary>
public class ForgotPasswordRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;
}

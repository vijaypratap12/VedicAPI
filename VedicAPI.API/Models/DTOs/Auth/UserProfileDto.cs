namespace VedicAPI.API.Models.DTOs.Auth;

/// <summary>
/// DTO for user profile information
/// </summary>
public class UserProfileDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? LastLoginAt { get; set; }
    
    public string? ProfileImageUrl { get; set; }
}


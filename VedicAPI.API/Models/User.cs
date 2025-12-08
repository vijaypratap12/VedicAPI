namespace VedicAPI.API.Models;

/// <summary>
/// User entity representing a registered user in the system
/// </summary>
public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? LastLoginAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public string? ProfileImageUrl { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiryTime { get; set; }
}


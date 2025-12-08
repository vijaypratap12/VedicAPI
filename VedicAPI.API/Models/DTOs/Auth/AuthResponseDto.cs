namespace VedicAPI.API.Models.DTOs.Auth;

/// <summary>
/// DTO for authentication response containing user info and tokens
/// </summary>
public class AuthResponseDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Token { get; set; } = string.Empty;
    
    public string RefreshToken { get; set; } = string.Empty;
    
    public DateTime TokenExpiry { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string? ProfileImageUrl { get; set; }
}


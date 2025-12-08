using VedicAPI.API.Models;

namespace VedicAPI.API.Repositories.Interfaces;

/// <summary>
/// Repository interface for authentication and user management
/// </summary>
public interface IAuthRepository
{
    /// <summary>
    /// Get user by email address
    /// </summary>
    Task<User?> GetUserByEmailAsync(string email);
    
    /// <summary>
    /// Get user by ID
    /// </summary>
    Task<User?> GetUserByIdAsync(int id);
    
    /// <summary>
    /// Create a new user
    /// </summary>
    Task<User> CreateUserAsync(User user);
    
    /// <summary>
    /// Update user's last login timestamp
    /// </summary>
    Task UpdateLastLoginAsync(int userId);
    
    /// <summary>
    /// Update user's refresh token
    /// </summary>
    Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime);
    
    /// <summary>
    /// Get user by refresh token
    /// </summary>
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    
    /// <summary>
    /// Revoke user's refresh token
    /// </summary>
    Task RevokeRefreshTokenAsync(int userId);
    
    /// <summary>
    /// Check if email already exists
    /// </summary>
    Task<bool> EmailExistsAsync(string email);
}


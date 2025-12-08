using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using VedicAPI.API.Models;
using VedicAPI.API.Repositories.Interfaces;

namespace VedicAPI.API.Repositories;

/// <summary>
/// Repository implementation for authentication and user management
/// </summary>
public class AuthRepository : IAuthRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public AuthRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    /// <inheritdoc/>
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var connection = CreateConnection();
        
        var user = await connection.QueryFirstOrDefaultAsync<User>(
            "sp_GetUserByEmail",
            new { Email = email },
            commandType: CommandType.StoredProcedure
        );

        return user;
    }

    /// <inheritdoc/>
    public async Task<User?> GetUserByIdAsync(int id)
    {
        using var connection = CreateConnection();
        
        var user = await connection.QueryFirstOrDefaultAsync<User>(
            "sp_GetUserById",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );

        return user;
    }

    /// <inheritdoc/>
    public async Task<User> CreateUserAsync(User user)
    {
        using var connection = CreateConnection();
        
        var createdUser = await connection.QuerySingleAsync<User>(
            "sp_CreateUser",
            new
            {
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                ProfileImageUrl = user.ProfileImageUrl
            },
            commandType: CommandType.StoredProcedure
        );

        return createdUser;
    }

    /// <inheritdoc/>
    public async Task UpdateLastLoginAsync(int userId)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "sp_UpdateLastLogin",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task UpdateRefreshTokenAsync(int userId, string refreshToken, DateTime expiryTime)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "sp_UpdateRefreshToken",
            new
            {
                UserId = userId,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = expiryTime
            },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        using var connection = CreateConnection();
        
        var user = await connection.QueryFirstOrDefaultAsync<User>(
            "sp_GetUserByRefreshToken",
            new { RefreshToken = refreshToken },
            commandType: CommandType.StoredProcedure
        );

        return user;
    }

    /// <inheritdoc/>
    public async Task RevokeRefreshTokenAsync(int userId)
    {
        using var connection = CreateConnection();
        
        await connection.ExecuteAsync(
            "sp_RevokeRefreshToken",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<bool> EmailExistsAsync(string email)
    {
        using var connection = CreateConnection();
        
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM USERS WHERE Email = @Email",
            new { Email = email }
        );

        return count > 0;
    }
}


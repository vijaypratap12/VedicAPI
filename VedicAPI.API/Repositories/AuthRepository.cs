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

    /// <inheritdoc/>
    public async Task<UserOTP> CreateUserOTPAsync(int? userId, string email, string otp, string otpType, DateTime expiry)
    {
        using var connection = CreateConnection();
        
        var userOTP = await connection.QuerySingleAsync<UserOTP>(
            "sp_CreateUserOTP",
            new
            {
                UserId = userId,
                Email = email,
                OTP = otp,
                OTPType = otpType,
                OTPExpiry = expiry
            },
            commandType: CommandType.StoredProcedure
        );

        return userOTP;
    }

    /// <inheritdoc/>
    public async Task<UserOTP?> VerifyUserOTPAsync(string email, string otp, string otpType)
    {
        using var connection = CreateConnection();
        
        var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
            "sp_VerifyUserOTP",
            new
            {
                Email = email,
                OTP = otp,
                OTPType = otpType
            },
            commandType: CommandType.StoredProcedure
        );

        if (result == null)
            return null;

        // Map the result to UserOTP using dynamic property access
        var userOTP = new UserOTP
        {
            Id = (int)result.Id,
            UserId = result.UserId != null ? (int?)result.UserId : null,
            Email = (string)result.Email,
            OTP = (string)result.OTP,
            OTPType = (string)result.OTPType,
            OTPExpiry = (DateTime)result.OTPExpiry,
            IsUsed = (bool)result.IsUsed,
            RequestedAt = (DateTime)result.RequestedAt,
            CreatedAt = (DateTime)result.CreatedAt,
            UpdatedAt = (DateTime)result.UpdatedAt
        };

        // If UserId exists, map User info
        if (result.User_Id != null)
        {
            userOTP.User = new User
            {
                Id = (int)result.User_Id,
                Name = (string)result.User_Name,
                Email = (string)result.User_Email,
                PasswordHash = (string)result.User_PasswordHash,
                CreatedAt = (DateTime)result.User_CreatedAt,
                LastLoginAt = result.User_LastLoginAt != null ? (DateTime?)result.User_LastLoginAt : null,
                IsActive = (bool)result.User_IsActive,
                ProfileImageUrl = result.User_ProfileImageUrl != null ? (string?)result.User_ProfileImageUrl : null
            };
        }

        return userOTP;
    }

    /// <inheritdoc/>
    public async Task<bool> ResetPasswordWithOTPAsync(string email, string otp, string newPasswordHash)
    {
        using var connection = CreateConnection();
        
        try
        {
            var result = await connection.QueryFirstOrDefaultAsync<int>(
                "sp_ResetPasswordWithOTP",
                new
                {
                    Email = email,
                    OTP = otp,
                    NewPasswordHash = newPasswordHash
                },
                commandType: CommandType.StoredProcedure
            );

            return result == 1;
        }
        catch (SqlException ex) when (ex.Message.Contains("Invalid or expired OTP"))
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<int> ClearExpiredUserOTPsAsync()
    {
        using var connection = CreateConnection();
        
        var deletedCount = await connection.ExecuteScalarAsync<int>(
            "sp_ClearExpiredUserOTPs",
            commandType: CommandType.StoredProcedure
        );

        return deletedCount;
    }
}


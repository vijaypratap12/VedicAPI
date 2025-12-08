-- ============================================
-- Create Users Table for Authentication
-- ============================================

USE VedicDB;
GO

-- Drop existing table if exists (for development only)
IF OBJECT_ID('dbo.USERS', 'U') IS NOT NULL
    DROP TABLE dbo.USERS;
GO

-- Create USERS table
CREATE TABLE dbo.USERS (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastLoginAt DATETIME2 NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    ProfileImageUrl NVARCHAR(500) NULL,
    RefreshToken NVARCHAR(500) NULL,
    RefreshTokenExpiryTime DATETIME2 NULL,
    
    CONSTRAINT CK_USERS_Email CHECK (Email LIKE '%@%.%'),
    CONSTRAINT CK_USERS_Name_Length CHECK (LEN(Name) >= 2)
);
GO

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX IX_USERS_Email 
    ON dbo.USERS(Email);
GO

CREATE NONCLUSTERED INDEX IX_USERS_IsActive 
    ON dbo.USERS(IsActive) 
    WHERE IsActive = 1;
GO

CREATE NONCLUSTERED INDEX IX_USERS_CreatedAt 
    ON dbo.USERS(CreatedAt DESC);
GO

-- ============================================
-- Stored Procedures for User Management
-- ============================================

-- Procedure: Get User by Email
IF OBJECT_ID('dbo.sp_GetUserByEmail', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetUserByEmail;
GO

CREATE PROCEDURE dbo.sp_GetUserByEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Name,
        Email,
        PasswordHash,
        CreatedAt,
        LastLoginAt,
        IsActive,
        ProfileImageUrl,
        RefreshToken,
        RefreshTokenExpiryTime
    FROM dbo.USERS
    WHERE Email = @Email AND IsActive = 1;
END;
GO

-- Procedure: Get User by ID
IF OBJECT_ID('dbo.sp_GetUserById', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetUserById;
GO

CREATE PROCEDURE dbo.sp_GetUserById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Name,
        Email,
        PasswordHash,
        CreatedAt,
        LastLoginAt,
        IsActive,
        ProfileImageUrl,
        RefreshToken,
        RefreshTokenExpiryTime
    FROM dbo.USERS
    WHERE Id = @Id AND IsActive = 1;
END;
GO

-- Procedure: Create User
IF OBJECT_ID('dbo.sp_CreateUser', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CreateUser;
GO

CREATE PROCEDURE dbo.sp_CreateUser
    @Name NVARCHAR(100),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(500),
    @ProfileImageUrl NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if email already exists
    IF EXISTS (SELECT 1 FROM dbo.USERS WHERE Email = @Email)
    BEGIN
        RAISERROR('Email already exists', 16, 1);
        RETURN;
    END
    
    -- Insert new user
    INSERT INTO dbo.USERS (Name, Email, PasswordHash, ProfileImageUrl, CreatedAt, IsActive)
    VALUES (@Name, @Email, @PasswordHash, @ProfileImageUrl, GETUTCDATE(), 1);
    
    -- Return the newly created user
    SELECT 
        Id,
        Name,
        Email,
        PasswordHash,
        CreatedAt,
        LastLoginAt,
        IsActive,
        ProfileImageUrl,
        RefreshToken,
        RefreshTokenExpiryTime
    FROM dbo.USERS
    WHERE Id = SCOPE_IDENTITY();
END;
GO

-- Procedure: Update Last Login
IF OBJECT_ID('dbo.sp_UpdateLastLogin', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_UpdateLastLogin;
GO

CREATE PROCEDURE dbo.sp_UpdateLastLogin
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.USERS
    SET LastLoginAt = GETUTCDATE()
    WHERE Id = @UserId;
END;
GO

-- Procedure: Update Refresh Token
IF OBJECT_ID('dbo.sp_UpdateRefreshToken', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_UpdateRefreshToken;
GO

CREATE PROCEDURE dbo.sp_UpdateRefreshToken
    @UserId INT,
    @RefreshToken NVARCHAR(500),
    @RefreshTokenExpiryTime DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.USERS
    SET 
        RefreshToken = @RefreshToken,
        RefreshTokenExpiryTime = @RefreshTokenExpiryTime
    WHERE Id = @UserId;
END;
GO

-- Procedure: Get User by Refresh Token
IF OBJECT_ID('dbo.sp_GetUserByRefreshToken', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetUserByRefreshToken;
GO

CREATE PROCEDURE dbo.sp_GetUserByRefreshToken
    @RefreshToken NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Name,
        Email,
        PasswordHash,
        CreatedAt,
        LastLoginAt,
        IsActive,
        ProfileImageUrl,
        RefreshToken,
        RefreshTokenExpiryTime
    FROM dbo.USERS
    WHERE RefreshToken = @RefreshToken 
        AND IsActive = 1
        AND RefreshTokenExpiryTime > GETUTCDATE();
END;
GO

-- Procedure: Revoke Refresh Token
IF OBJECT_ID('dbo.sp_RevokeRefreshToken', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_RevokeRefreshToken;
GO

CREATE PROCEDURE dbo.sp_RevokeRefreshToken
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.USERS
    SET 
        RefreshToken = NULL,
        RefreshTokenExpiryTime = NULL
    WHERE Id = @UserId;
END;
GO

-- ============================================
-- Sample Data (Optional - for testing)
-- ============================================

-- Note: In production, users will be created through the signup endpoint
-- This is just for testing purposes

PRINT 'USERS table and stored procedures created successfully!';
GO


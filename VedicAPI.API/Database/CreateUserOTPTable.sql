-- ============================================
-- Create USER_OTP Table for OTP Management
-- Generic table for all OTP features (PasswordReset, Signup, EmailVerification, etc.)
-- ============================================

USE VedicDB;
GO

-- Drop existing table if exists (for development only)
IF OBJECT_ID('dbo.USER_OTP', 'U') IS NOT NULL
    DROP TABLE dbo.USER_OTP;
GO

-- Create USER_OTP table
CREATE TABLE dbo.USER_OTP (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL, 
    Email NVARCHAR(255) NOT NULL,  -- Required for signup OTP
    OTP NVARCHAR(10) NULL,  -- 6-digit code
    OTPType NVARCHAR(50) NOT NULL,  -- 'PasswordReset', 'Signup', 'EmailVerification', etc.
    OTPExpiry DATETIME2 NOT NULL,
    IsUsed BIT NOT NULL DEFAULT 0,
    RequestedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_USER_OTP_USERS FOREIGN KEY (UserId) 
        REFERENCES dbo.USERS(Id) ON DELETE CASCADE,
    CONSTRAINT CK_USER_OTP_UserIdOrEmail CHECK (
        (UserId IS NOT NULL) OR (Email IS NOT NULL)
    )
);
GO

-- Create indexes for better performance
CREATE NONCLUSTERED INDEX IX_USER_OTP_UserId 
    ON dbo.USER_OTP(UserId) 
    WHERE UserId IS NOT NULL;
GO

CREATE NONCLUSTERED INDEX IX_USER_OTP_Email 
    ON dbo.USER_OTP(Email);
GO

CREATE NONCLUSTERED INDEX IX_USER_OTP_OTP 
    ON dbo.USER_OTP(OTP);
GO

CREATE NONCLUSTERED INDEX IX_USER_OTP_OTPType 
    ON dbo.USER_OTP(OTPType);
GO

CREATE NONCLUSTERED INDEX IX_USER_OTP_Email_OTP_OTPType 
    ON dbo.USER_OTP(Email, OTP, OTPType) 
    WHERE IsUsed = 0 AND OTPExpiry > GETUTCDATE();
GO

-- ============================================
-- Stored Procedures for OTP Management
-- ============================================

-- Procedure: Create User OTP
IF OBJECT_ID('dbo.sp_CreateUserOTP', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CreateUserOTP;
GO

CREATE PROCEDURE dbo.sp_CreateUserOTP
    @UserId INT = NULL,
    @Email NVARCHAR(255),
    @OTP NVARCHAR(10),
    @OTPType NVARCHAR(50),
    @OTPExpiry DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validate that either UserId or Email is provided
    IF @UserId IS NULL AND (@Email IS NULL OR @Email = '')
    BEGIN
        RAISERROR('Either UserId or Email must be provided', 16, 1);
        RETURN;
    END
    
    -- Insert new OTP record
    INSERT INTO dbo.USER_OTP (UserId, Email, OTP, OTPType, OTPExpiry, RequestedAt, CreatedAt, UpdatedAt)
    VALUES (@UserId, @Email, @OTP, @OTPType, @OTPExpiry, GETUTCDATE(), GETUTCDATE(), GETUTCDATE());
    
    -- Return the newly created OTP record
    SELECT 
        Id,
        UserId,
        Email,
        OTP,
        OTPType,
        OTPExpiry,
        IsUsed,
        RequestedAt,
        CreatedAt,
        UpdatedAt
    FROM dbo.USER_OTP
    WHERE Id = SCOPE_IDENTITY();
END;
GO

-- Procedure: Verify User OTP
IF OBJECT_ID('dbo.sp_VerifyUserOTP', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_VerifyUserOTP;
GO

CREATE PROCEDURE dbo.sp_VerifyUserOTP
    @Email NVARCHAR(255),
    @OTP NVARCHAR(10),
    @OTPType NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        uotp.Id,
        uotp.UserId,
        uotp.Email,
        uotp.OTP,
        uotp.OTPType,
        uotp.OTPExpiry,
        uotp.IsUsed,
        uotp.RequestedAt,
        uotp.CreatedAt,
        uotp.UpdatedAt,
        -- Include User info if UserId exists
        u.Id AS User_Id,
        u.Name AS User_Name,
        u.Email AS User_Email,
        u.PasswordHash AS User_PasswordHash,
        u.CreatedAt AS User_CreatedAt,
        u.LastLoginAt AS User_LastLoginAt,
        u.IsActive AS User_IsActive,
        u.ProfileImageUrl AS User_ProfileImageUrl
    FROM dbo.USER_OTP uotp
    LEFT JOIN dbo.USERS u ON uotp.UserId = u.Id
    WHERE uotp.Email = @Email
        AND uotp.OTP = @OTP
        AND uotp.OTPType = @OTPType
        AND uotp.IsUsed = 0
        AND uotp.OTPExpiry > GETUTCDATE();
END;
GO

-- Procedure: Reset Password with OTP
IF OBJECT_ID('dbo.sp_ResetPasswordWithOTP', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ResetPasswordWithOTP;
GO

CREATE PROCEDURE dbo.sp_ResetPasswordWithOTP
    @Email NVARCHAR(255),
    @OTP NVARCHAR(10),
    @NewPasswordHash NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Verify OTP exists and is valid
        DECLARE @UserId INT;
        DECLARE @OTPId INT;
        
        SELECT @UserId = UserId, @OTPId = Id
        FROM dbo.USER_OTP
        WHERE Email = @Email
            AND OTP = @OTP
            AND OTPType = 'PasswordReset'
            AND IsUsed = 0
            AND OTPExpiry > GETUTCDATE();
        
        -- If OTP not found or invalid, return error
        IF @OTPId IS NULL OR @UserId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Invalid or expired OTP', 16, 1);
            RETURN;
        END
        
        -- Update user password
        UPDATE dbo.USERS
        SET PasswordHash = @NewPasswordHash
        WHERE Id = @UserId;
        
        -- Mark OTP as used
        UPDATE dbo.USER_OTP
        SET IsUsed = 1,
            UpdatedAt = GETUTCDATE()
        WHERE Id = @OTPId;
        
        COMMIT TRANSACTION;
        
        -- Return success indicator
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO

-- Procedure: Clear Expired User OTPs (Maintenance)
IF OBJECT_ID('dbo.sp_ClearExpiredUserOTPs', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ClearExpiredUserOTPs;
GO

CREATE PROCEDURE dbo.sp_ClearExpiredUserOTPs
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Delete expired OTPs that are either used or expired more than 24 hours ago
    DELETE FROM dbo.USER_OTP
    WHERE (IsUsed = 1 AND UpdatedAt < DATEADD(HOUR, -24, GETUTCDATE()))
       OR (OTPExpiry < GETUTCDATE() AND CreatedAt < DATEADD(HOUR, -24, GETUTCDATE()));
    
    -- Return count of deleted records
    SELECT @@ROWCOUNT AS DeletedCount;
END;
GO

PRINT 'USER_OTP table and stored procedures created successfully!';
GO

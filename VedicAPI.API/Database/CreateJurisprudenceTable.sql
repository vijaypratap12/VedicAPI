-- =============================================
-- Vedic API - Jurisprudence (Legal & Policy Documents) Database Schema
-- =============================================
-- Description: Creates table and stored procedures for managing
--              legal documents, guidelines, regulations, and court rulings
-- Author: Vedic AI Team
-- Date: 2025
-- =============================================

USE VedicDB;
GO

PRINT '========================================';
PRINT 'Creating Jurisprudence Schema';
PRINT '========================================';

-- =============================================
-- Step 1: Create JURISPRUDENCE Table
-- =============================================
PRINT 'Step 1: Creating JURISPRUDENCE table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'JURISPRUDENCE')
BEGIN
    CREATE TABLE JURISPRUDENCE (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(500) NOT NULL,
        Type NVARCHAR(50) NOT NULL,  -- guideline, regulation, ruling, ethical
        Date NVARCHAR(20) NULL,       -- e.g. "2024" or "2023-06"
        Description NVARCHAR(MAX) NULL,
        DocumentUrl NVARCHAR(500) NULL,  -- URL or path to uploaded file
        State NVARCHAR(100) NULL,     -- optional, for state-specific docs
        DisplayOrder INT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        
        INDEX IX_JURISPRUDENCE_Type (Type),
        INDEX IX_JURISPRUDENCE_IsActive (IsActive),
        INDEX IX_JURISPRUDENCE_DisplayOrder (DisplayOrder),
        INDEX IX_JURISPRUDENCE_CreatedAt (CreatedAt DESC)
    );
    PRINT '  ✓ JURISPRUDENCE table created successfully';
END
ELSE
BEGIN
    PRINT '  ℹ JURISPRUDENCE table already exists';
END
GO

-- =============================================
-- Step 2: Create Stored Procedures
-- =============================================
PRINT 'Step 2: Creating stored procedures...';

-- Get paged jurisprudence items with total count (returns 2 result sets)
CREATE OR ALTER PROCEDURE sp_GetJurisprudencePaged
    @SearchTerm NVARCHAR(200) = NULL,
    @Type NVARCHAR(50) = NULL,
    @IncludeInactive BIT = 0,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    -- First result set: Total count
    SELECT COUNT(*) AS TotalCount
    FROM JURISPRUDENCE
    WHERE (@IncludeInactive = 1 OR IsActive = 1)
        AND (@Type IS NULL OR Type = @Type)
        AND (@SearchTerm IS NULL OR 
             Title LIKE '%' + @SearchTerm + '%' OR
             Description LIKE '%' + @SearchTerm + '%');
    
    -- Second result set: Paged items
    SELECT 
        Id, Title, Type, Date, Description, DocumentUrl, State,
        DisplayOrder, IsActive, CreatedAt, UpdatedAt
    FROM JURISPRUDENCE
    WHERE (@IncludeInactive = 1 OR IsActive = 1)
        AND (@Type IS NULL OR Type = @Type)
        AND (@SearchTerm IS NULL OR 
             Title LIKE '%' + @SearchTerm + '%' OR
             Description LIKE '%' + @SearchTerm + '%')
    ORDER BY DisplayOrder ASC, CreatedAt DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- Get jurisprudence item by ID
CREATE OR ALTER PROCEDURE sp_GetJurisprudenceById
    @Id INT,
    @IncludeInactive BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id, Title, Type, Date, Description, DocumentUrl, State,
        DisplayOrder, IsActive, CreatedAt, UpdatedAt
    FROM JURISPRUDENCE
    WHERE Id = @Id
        AND (@IncludeInactive = 1 OR IsActive = 1);
END
GO

-- Create new jurisprudence item
CREATE OR ALTER PROCEDURE sp_CreateJurisprudence
    @Title NVARCHAR(500),
    @Type NVARCHAR(50),
    @Date NVARCHAR(20) = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @DocumentUrl NVARCHAR(500) = NULL,
    @State NVARCHAR(100) = NULL,
    @DisplayOrder INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO JURISPRUDENCE (
        Title, Type, Date, Description, DocumentUrl, State, DisplayOrder,
        IsActive, CreatedAt, UpdatedAt
    )
    VALUES (
        @Title, @Type, @Date, @Description, @DocumentUrl, @State, @DisplayOrder,
        1, GETUTCDATE(), GETUTCDATE()
    );
    
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

-- Update jurisprudence item
CREATE OR ALTER PROCEDURE sp_UpdateJurisprudence
    @Id INT,
    @Title NVARCHAR(500),
    @Type NVARCHAR(50),
    @Date NVARCHAR(20) = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @DocumentUrl NVARCHAR(500) = NULL,
    @State NVARCHAR(100) = NULL,
    @DisplayOrder INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE JURISPRUDENCE
    SET Title = @Title,
        Type = @Type,
        Date = @Date,
        Description = @Description,
        DocumentUrl = @DocumentUrl,
        State = @State,
        DisplayOrder = @DisplayOrder,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- Soft delete jurisprudence item
CREATE OR ALTER PROCEDURE sp_DeleteJurisprudence
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE JURISPRUDENCE
    SET IsActive = 0,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

PRINT '  ✓ Stored procedures created successfully';
GO

PRINT '========================================';
PRINT 'Jurisprudence Schema Creation Complete';
PRINT '========================================';

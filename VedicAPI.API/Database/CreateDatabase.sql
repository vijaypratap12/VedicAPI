-- =============================================
-- Vedic API Database Schema
-- Database: VedicDB
-- Description: Database schema for storing book content
-- =============================================

-- Create Database (Run this separately if database doesn't exist)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'VedicDB')
BEGIN
    CREATE DATABASE Dev_VedicAI;
END
GO

USE VedicDB;
GO

-- =============================================
-- Create Books Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Books')
BEGIN
    CREATE TABLE Books (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(500) NOT NULL,
        Author NVARCHAR(200) NOT NULL,
        Description NVARCHAR(2000) NULL,
        Content NVARCHAR(MAX) NOT NULL,
        Category NVARCHAR(100) NULL,
        Language NVARCHAR(50) NULL,
        PublicationYear INT NULL,
        ISBN NVARCHAR(20) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        
        -- Indexes for better query performance
        INDEX IX_Books_Category (Category),
        INDEX IX_Books_IsActive (IsActive),
        INDEX IX_Books_CreatedAt (CreatedAt DESC)
    );
    
    PRINT 'Books table created successfully';
END
ELSE
BEGIN
    PRINT 'Books table already exists';
END
GO

-- =============================================
-- Create Full-Text Search Index (Optional but recommended for large content)
-- =============================================
-- Note: Full-text search requires additional SQL Server configuration
-- Uncomment the following if you want to enable full-text search

/*
IF NOT EXISTS (SELECT * FROM sys.fulltext_catalogs WHERE name = 'VedicBooksCatalog')
BEGIN
    CREATE FULLTEXT CATALOG VedicBooksCatalog AS DEFAULT;
    PRINT 'Full-text catalog created successfully';
END
GO

IF NOT EXISTS (SELECT * FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID('Books'))
BEGIN
    CREATE FULLTEXT INDEX ON Books(Title, Author, Description, Content)
    KEY INDEX PK__Books__3214EC07 ON VedicBooksCatalog
    WITH CHANGE_TRACKING AUTO;
    PRINT 'Full-text index created successfully';
END
GO
*/

-- =============================================
-- Insert Sample Data (Optional)
-- =============================================
IF NOT EXISTS (SELECT * FROM Books)
BEGIN
    INSERT INTO Books (Title, Author, Description, Content, Category, Language, PublicationYear, ISBN, CreatedAt, IsActive)
    VALUES 
    (
        'Sushruta Samhita - Sutrasthana',
        'Acharya Sushruta',
        'The foundational text of Ayurvedic surgery, containing principles and practices of ancient Indian surgical techniques.',
        'The Sushruta Samhita is one of the most important ancient texts on medicine and surgery. It describes over 300 surgical procedures and 120 surgical instruments. The text is divided into six sections: Sutrasthana (general principles), Nidanasthana (pathology), Sharirasthana (anatomy), Chikitsasthana (therapeutics), Kalpasthana (toxicology), and Uttaratantra (specialized treatments).',
        'Ayurveda',
        'Sanskrit',
        600,
        NULL,
        GETUTCDATE(),
        1
    ),
    (
        'Charaka Samhita',
        'Acharya Charaka',
        'A comprehensive text on Ayurvedic medicine, covering diagnosis, treatment, and prevention of diseases.',
        'The Charaka Samhita is one of the two foundational texts of Ayurveda, the other being Sushruta Samhita. It emphasizes the importance of digestion, metabolism, and immunity in maintaining health. The text contains detailed descriptions of various diseases, their causes, symptoms, and treatments using herbs, diet, and lifestyle modifications.',
        'Ayurveda',
        'Sanskrit',
        400,
        NULL,
        GETUTCDATE(),
        1
    ),
    (
        'Ashtanga Hridaya',
        'Vagbhata',
        'A concise compilation of Ayurvedic knowledge, combining the teachings of Charaka and Sushruta.',
        'The Ashtanga Hridaya is a comprehensive medical text that synthesizes the knowledge from both Charaka Samhita and Sushruta Samhita. It is written in verse form and covers all eight branches of Ayurveda: internal medicine, surgery, ENT and ophthalmology, pediatrics, toxicology, psychiatry, rejuvenation, and aphrodisiacs.',
        'Ayurveda',
        'Sanskrit',
        700,
        NULL,
        GETUTCDATE(),
        1
    );
    
    PRINT 'Sample data inserted successfully';
END
ELSE
BEGIN
    PRINT 'Sample data already exists';
END
GO

-- =============================================
-- Create Stored Procedures (Optional - for advanced scenarios)
-- =============================================

-- Procedure to get books with pagination
CREATE OR ALTER PROCEDURE sp_GetBooksPaginated
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @Category NVARCHAR(100) = NULL,
    @SearchTerm NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    SELECT 
        Id, Title, Author, Description, Content, Category, Language,
        PublicationYear, ISBN, CreatedAt, UpdatedAt, IsActive,
        COUNT(*) OVER() AS TotalCount
    FROM Books
    WHERE 
        IsActive = 1
        AND (@Category IS NULL OR Category = @Category)
        AND (@SearchTerm IS NULL OR 
             Title LIKE '%' + @SearchTerm + '%' OR
             Author LIKE '%' + @SearchTerm + '%' OR
             Description LIKE '%' + @SearchTerm + '%')
    ORDER BY CreatedAt DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

PRINT 'Database schema created successfully!';
PRINT 'Connection String: Server=localhost;Database=VedicDB;Trusted_Connection=True;TrustServerCertificate=True;';
GO


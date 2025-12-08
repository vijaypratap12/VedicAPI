-- =============================================
-- Vedic API Database - Textbooks System
-- Description: Create separate Textbooks table and related objects
-- =============================================

USE VedicDB;
GO

PRINT 'Creating Textbooks system...';
GO

-- =============================================
-- Step 1: Create TEXTBOOKS Table
-- =============================================
PRINT 'Step 1: Creating TEXTBOOKS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TEXTBOOKS')
BEGIN
    CREATE TABLE TEXTBOOKS (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(500) NOT NULL,
        Author NVARCHAR(200) NOT NULL,
        Description NVARCHAR(2000) NULL,
        CoverImageUrl NVARCHAR(500) NULL,
        TotalChapters INT NOT NULL DEFAULT 0,
        
        -- Basic Information
        Category NVARCHAR(100) NULL,
        Language NVARCHAR(50) NULL,
        PublicationYear INT NULL,
        ISBN NVARCHAR(20) NULL,
        
        -- Textbook-specific fields
        Rating DECIMAL(3,2) NULL DEFAULT 4.5,
        DownloadCount INT NOT NULL DEFAULT 0,
        ViewCount INT NOT NULL DEFAULT 0,
        Status NVARCHAR(50) NULL DEFAULT N'completed',
        Tags NVARCHAR(1000) NULL,
        Level NVARCHAR(50) NULL,
        Year NVARCHAR(50) NULL,
        PageCount INT NULL,
        
        -- Audit fields
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        
        -- Indexes for better query performance
        INDEX IX_Textbooks_Category (Category),
        INDEX IX_Textbooks_IsActive (IsActive),
        INDEX IX_Textbooks_Level (Level),
        INDEX IX_Textbooks_Status (Status),
        INDEX IX_Textbooks_Rating (Rating DESC),
        INDEX IX_Textbooks_CreatedAt (CreatedAt DESC),
        INDEX IX_Textbooks_Category_IsActive_Rating (Category, IsActive, Rating DESC)
    );
    
    PRINT 'TEXTBOOKS table created successfully!';
END
ELSE
BEGIN
    PRINT 'TEXTBOOKS table already exists';
END
GO

-- =============================================
-- Step 2: Create TEXTBOOKCHAPTERS Table
-- =============================================
PRINT 'Step 2: Creating TEXTBOOKCHAPTERS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TEXTBOOKCHAPTERS')
BEGIN
    CREATE TABLE TEXTBOOKCHAPTERS (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TextbookId INT NOT NULL,
        ChapterNumber INT NOT NULL,
        ChapterTitle NVARCHAR(500) NOT NULL,
        ChapterSubtitle NVARCHAR(500) NULL,
        ContentHtml NVARCHAR(MAX) NOT NULL,
        Summary NVARCHAR(2000) NULL,
        WordCount INT NULL,
        ReadingTimeMinutes INT NULL,
        DisplayOrder INT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        
        -- Foreign Key
        CONSTRAINT FK_TextbookChapters_Textbooks FOREIGN KEY (TextbookId) 
            REFERENCES TEXTBOOKS(Id) ON DELETE CASCADE,
        
        -- Unique constraint
        CONSTRAINT UQ_TextbookChapter UNIQUE (TextbookId, ChapterNumber)
    );
    
    -- Create Indexes
    CREATE INDEX IX_TextbookChapters_TextbookId ON TEXTBOOKCHAPTERS(TextbookId);
    CREATE INDEX IX_TextbookChapters_DisplayOrder ON TEXTBOOKCHAPTERS(TextbookId, DisplayOrder);
    CREATE INDEX IX_TextbookChapters_ChapterNumber ON TEXTBOOKCHAPTERS(TextbookId, ChapterNumber);
    
    PRINT 'TEXTBOOKCHAPTERS table created successfully!';
END
ELSE
BEGIN
    PRINT 'TEXTBOOKCHAPTERS table already exists';
END
GO

-- =============================================
-- Step 3: Insert Sample Textbook Data
-- =============================================
PRINT 'Step 3: Inserting sample textbook data...';

-- Insert UG Textbooks
IF NOT EXISTS (SELECT * FROM TEXTBOOKS WHERE Title = N'Fundamentals of Shalya Tantra')
BEGIN
    INSERT INTO TEXTBOOKS (
        Title, Author, Description, Category, Language, PublicationYear, 
        ISBN, Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, 
        PageCount, TotalChapters, CoverImageUrl, CreatedAt, IsActive
    )
    VALUES 
    (
        N'Fundamentals of Shalya Tantra',
        N'Dr. Sameep Singh & Dr. Saurabh Kumar',
        N'Introduction to Ayurvedic Surgery covering basic principles, history, instruments, and anatomy. A comprehensive guide for BAMS first-year students aligned with NCISM curriculum.',
        N'Surgery',
        N'English, Hindi',
        2024,
        N'978-93-XXXXX-01-1',
        4.8,
        1250,
        3500,
        N'completed',
        N'Basic Principles,History,Instruments,Anatomy,Shalya Tantra,Surgery Basics',
        N'UG',
        N'1st Year',
        280,
        12,
        N'https://example.com/covers/fundamentals-shalya.jpg',
        GETUTCDATE(),
        1
    );
    PRINT '  - Inserted: Fundamentals of Shalya Tantra';
END

IF NOT EXISTS (SELECT * FROM TEXTBOOKS WHERE Title = N'Practical Shalya Tantra')
BEGIN
    INSERT INTO TEXTBOOKS (
        Title, Author, Description, Category, Language, PublicationYear, 
        ISBN, Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, 
        PageCount, TotalChapters, CoverImageUrl, CreatedAt, IsActive
    )
    VALUES 
    (
        N'Practical Shalya Tantra',
        N'Dr. Sameep Singh & Dr. Saurabh Kumar',
        N'Clinical Applications & Procedures focusing on wound management, basic procedures, and diagnosis. Essential reading for BAMS second-year students.',
        N'Surgery',
        N'English, Hindi',
        2024,
        N'978-93-XXXXX-02-8',
        4.7,
        980,
        2800,
        N'completed',
        N'Wound Management,Basic Procedures,Diagnosis,Clinical Practice',
        N'UG',
        N'2nd Year',
        350,
        15,
        N'https://example.com/covers/practical-shalya.jpg',
        GETUTCDATE(),
        1
    );
    PRINT '  - Inserted: Practical Shalya Tantra';
END

IF NOT EXISTS (SELECT * FROM TEXTBOOKS WHERE Title = N'Advanced Shalya Tantra')
BEGIN
    INSERT INTO TEXTBOOKS (
        Title, Author, Description, Category, Language, PublicationYear, 
        ISBN, Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, 
        PageCount, TotalChapters, CoverImageUrl, CreatedAt, IsActive
    )
    VALUES 
    (
        N'Advanced Shalya Tantra',
        N'Dr. Sameep Singh & Dr. Saurabh Kumar',
        N'Specialized Techniques & Modern Integration covering advanced procedures, modern surgical integration, and research methodologies.',
        N'Surgery',
        N'English, Hindi',
        2024,
        N'978-93-XXXXX-03-5',
        4.9,
        756,
        2100,
        N'in-progress',
        N'Advanced Procedures,Modern Integration,Research,Specialized Techniques',
        N'UG',
        N'3rd Year',
        420,
        18,
        N'https://example.com/covers/advanced-shalya.jpg',
        GETUTCDATE(),
        1
    );
    PRINT '  - Inserted: Advanced Shalya Tantra';
END

-- Insert PG Textbooks
IF NOT EXISTS (SELECT * FROM TEXTBOOKS WHERE Title = N'Classical Shalya Tantra')
BEGIN
    INSERT INTO TEXTBOOKS (
        Title, Author, Description, Category, Language, PublicationYear, 
        ISBN, Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, 
        PageCount, TotalChapters, CoverImageUrl, CreatedAt, IsActive
    )
    VALUES 
    (
        N'Classical Shalya Tantra',
        N'Dr. Saurabh Kumar & Team',
        N'In-depth Study of Traditional Texts covering classical literature, research methodology, and advanced theory for MS Shalya Tantra scholars.',
        N'Surgery',
        N'English, Sanskrit, Hindi',
        2024,
        N'978-93-XXXXX-04-2',
        4.9,
        445,
        1200,
        N'completed',
        N'Classical Texts,Research Methodology,Advanced Theory,Sanskrit Literature',
        N'PG',
        N'1st Year PG',
        650,
        25,
        N'https://example.com/covers/classical-shalya.jpg',
        GETUTCDATE(),
        1
    );
    PRINT '  - Inserted: Classical Shalya Tantra';
END

IF NOT EXISTS (SELECT * FROM TEXTBOOKS WHERE Title = N'Modern Surgical Integration')
BEGIN
    INSERT INTO TEXTBOOKS (
        Title, Author, Description, Category, Language, PublicationYear, 
        ISBN, Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, 
        PageCount, TotalChapters, CoverImageUrl, CreatedAt, IsActive
    )
    VALUES 
    (
        N'Modern Surgical Integration',
        N'Dr. Sameep Singh & Advisory Panel',
        N'Bridging Ancient Wisdom with Contemporary Practice. Covers modern techniques, clinical research, and evidence-based practice in Ayurvedic surgery.',
        N'Surgery',
        N'English, Hindi',
        2024,
        N'978-93-XXXXX-05-9',
        4.8,
        312,
        890,
        N'completed',
        N'Modern Techniques,Clinical Research,Evidence-based Practice,Contemporary Surgery',
        N'PG',
        N'2nd Year PG',
        580,
        20,
        N'https://example.com/covers/modern-integration.jpg',
        GETUTCDATE(),
        1
    );
    PRINT '  - Inserted: Modern Surgical Integration';
END

IF NOT EXISTS (SELECT * FROM TEXTBOOKS WHERE Title = N'Thesis & Research Guide')
BEGIN
    INSERT INTO TEXTBOOKS (
        Title, Author, Description, Category, Language, PublicationYear, 
        ISBN, Rating, DownloadCount, ViewCount, Status, Tags, Level, Year, 
        PageCount, TotalChapters, CoverImageUrl, CreatedAt, IsActive
    )
    VALUES 
    (
        N'Thesis & Research Guide',
        N'Dr. Sameep Singh & Research Team',
        N'Complete Guide for PG Research covering research design, data analysis, publication strategies, and thesis writing for MS Shalya Tantra scholars.',
        N'Research',
        N'English',
        2024,
        N'978-93-XXXXX-06-6',
        4.7,
        278,
        750,
        N'in-progress',
        N'Research Design,Data Analysis,Publication,Thesis Writing,Academic Research',
        N'PG',
        N'Final Year PG',
        320,
        15,
        N'https://example.com/covers/research-guide.jpg',
        GETUTCDATE(),
        1
    );
    PRINT '  - Inserted: Thesis & Research Guide';
END

PRINT 'Sample textbook data inserted successfully!';
GO

-- =============================================
-- Step 4: Create Stored Procedures
-- =============================================
PRINT 'Step 4: Creating stored procedures...';

-- Procedure to get textbooks with filters
CREATE OR ALTER PROCEDURE sp_GetTextbooks
    @Level NVARCHAR(50) = NULL,
    @Category NVARCHAR(100) = NULL,
    @Status NVARCHAR(50) = NULL,
    @MinRating DECIMAL(3,2) = NULL,
    @SearchTerm NVARCHAR(200) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    SELECT 
        Id, Title, Author, Description, Category, Language,
        PublicationYear, ISBN, Rating, DownloadCount, ViewCount,
        Status, Tags, Level, Year, PageCount, TotalChapters,
        CoverImageUrl, CreatedAt, UpdatedAt, IsActive,
        COUNT(*) OVER() AS TotalCount
    FROM TEXTBOOKS
    WHERE 
        IsActive = 1
        AND (@Level IS NULL OR Level = @Level)
        AND (@Category IS NULL OR Category = @Category)
        AND (@Status IS NULL OR Status = @Status)
        AND (@MinRating IS NULL OR Rating >= @MinRating)
        AND (@SearchTerm IS NULL OR 
             Title LIKE N'%' + @SearchTerm + N'%' OR
             Author LIKE N'%' + @SearchTerm + N'%' OR
             Description LIKE N'%' + @SearchTerm + N'%' OR
             Tags LIKE N'%' + @SearchTerm + N'%')
    ORDER BY Rating DESC, CreatedAt DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- Procedure to increment download count
CREATE OR ALTER PROCEDURE sp_IncrementTextbookDownloadCount
    @TextbookId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TEXTBOOKS
    SET DownloadCount = DownloadCount + 1,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @TextbookId;
    
    SELECT DownloadCount FROM TEXTBOOKS WHERE Id = @TextbookId;
END
GO

-- Procedure to increment view count
CREATE OR ALTER PROCEDURE sp_IncrementTextbookViewCount
    @TextbookId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE TEXTBOOKS
    SET ViewCount = ViewCount + 1,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @TextbookId;
    
    SELECT ViewCount FROM TEXTBOOKS WHERE Id = @TextbookId;
END
GO

-- Procedure to update textbook rating
CREATE OR ALTER PROCEDURE sp_UpdateTextbookRating
    @TextbookId INT,
    @NewRating DECIMAL(3,2)
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @NewRating >= 0 AND @NewRating <= 5
    BEGIN
        UPDATE TEXTBOOKS
        SET Rating = @NewRating,
            UpdatedAt = GETUTCDATE()
        WHERE Id = @TextbookId;
        
        SELECT Rating FROM TEXTBOOKS WHERE Id = @TextbookId;
    END
    ELSE
    BEGIN
        RAISERROR('Rating must be between 0 and 5', 16, 1);
    END
END
GO

-- Procedure to get textbook statistics
CREATE OR ALTER PROCEDURE sp_GetTextbookStatistics
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        COUNT(*) AS TotalTextbooks,
        SUM(TotalChapters) AS TotalChapters,
        AVG(Rating) AS AverageRating,
        SUM(DownloadCount) AS TotalDownloads,
        SUM(ViewCount) AS TotalViews,
        COUNT(CASE WHEN Level = N'UG' THEN 1 END) AS UGTextbooks,
        COUNT(CASE WHEN Level = N'PG' THEN 1 END) AS PGTextbooks,
        COUNT(CASE WHEN Status = N'completed' THEN 1 END) AS CompletedTextbooks,
        COUNT(CASE WHEN Status = N'in-progress' THEN 1 END) AS InProgressTextbooks
    FROM TEXTBOOKS
    WHERE IsActive = 1;
END
GO

PRINT 'Stored procedures created successfully!';
GO

-- =============================================
-- Step 5: Create Views
-- =============================================
PRINT 'Step 5: Creating views...';

-- View for popular textbooks
CREATE OR ALTER VIEW vw_PopularTextbooks
AS
SELECT TOP 10
    Id, Title, Author, Description, Category, Language,
    Rating, DownloadCount, ViewCount, Level, Year,
    TotalChapters, CoverImageUrl
FROM TEXTBOOKS
WHERE IsActive = 1
ORDER BY (DownloadCount + ViewCount) DESC, Rating DESC;
GO

-- View for top-rated textbooks
CREATE OR ALTER VIEW vw_TopRatedTextbooks
AS
SELECT TOP 10
    Id, Title, Author, Description, Category, Language,
    Rating, DownloadCount, ViewCount, Level, Year,
    TotalChapters, CoverImageUrl
FROM TEXTBOOKS
WHERE IsActive = 1 AND Rating IS NOT NULL
ORDER BY Rating DESC, DownloadCount DESC;
GO

-- View for recently added textbooks
CREATE OR ALTER VIEW vw_RecentTextbooks
AS
SELECT TOP 10
    Id, Title, Author, Description, Category, Language,
    Rating, DownloadCount, ViewCount, Level, Year,
    TotalChapters, CoverImageUrl, CreatedAt
FROM TEXTBOOKS
WHERE IsActive = 1
ORDER BY CreatedAt DESC;
GO

PRINT 'Views created successfully!';
GO

-- =============================================
-- Verification Queries
-- =============================================
PRINT '';
PRINT '========================================';
PRINT 'Textbooks System Created Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Summary:';
SELECT 
    'Total Textbooks' AS Metric,
    COUNT(*) AS Value
FROM TEXTBOOKS
WHERE IsActive = 1
UNION ALL
SELECT 
    'Total Chapters' AS Metric,
    SUM(TotalChapters) AS Value
FROM TEXTBOOKS
WHERE IsActive = 1
UNION ALL
SELECT 
    'Average Rating' AS Metric,
    CAST(AVG(Rating) AS DECIMAL(3,2)) AS Value
FROM TEXTBOOKS
WHERE IsActive = 1 AND Rating IS NOT NULL
UNION ALL
SELECT 
    'UG Textbooks' AS Metric,
    COUNT(*) AS Value
FROM TEXTBOOKS
WHERE IsActive = 1 AND Level = N'UG'
UNION ALL
SELECT 
    'PG Textbooks' AS Metric,
    COUNT(*) AS Value
FROM TEXTBOOKS
WHERE IsActive = 1 AND Level = N'PG';
GO

PRINT '';
PRINT 'Available stored procedures:';
PRINT '  - sp_GetTextbooks - Get textbooks with filters';
PRINT '  - sp_IncrementTextbookDownloadCount - Track downloads';
PRINT '  - sp_IncrementTextbookViewCount - Track views';
PRINT '  - sp_UpdateTextbookRating - Update ratings';
PRINT '  - sp_GetTextbookStatistics - Get overall statistics';
PRINT '';
PRINT 'Available views:';
PRINT '  - vw_PopularTextbooks - Top 10 popular textbooks';
PRINT '  - vw_TopRatedTextbooks - Top 10 rated textbooks';
PRINT '  - vw_RecentTextbooks - Recently added textbooks';
PRINT '';
GO


-- =============================================
-- Vedic API - Research Papers & Thesis Repository Database Schema
-- =============================================
-- Description: Creates tables and stored procedures for managing
--              research papers and thesis documents
-- Author: Vedic AI Team
-- Date: 2024
-- =============================================

USE VedicDB;
GO

PRINT '========================================';
PRINT 'Creating Research & Thesis Repository Schema';
PRINT '========================================';

-- =============================================
-- Step 1: Create RESEARCH_PAPERS Table
-- =============================================
PRINT 'Step 1: Creating RESEARCH_PAPERS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RESEARCH_PAPERS')
BEGIN
    CREATE TABLE RESEARCH_PAPERS (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(500) NOT NULL,
        Authors NVARCHAR(1000) NOT NULL, -- Comma-separated list
        Institution NVARCHAR(300) NOT NULL,
        Year INT NOT NULL,
        Category NVARCHAR(100) NOT NULL, -- research-paper, case-study, review-article, clinical-trial
        Abstract NVARCHAR(MAX) NOT NULL,
        ContentHtml NVARCHAR(MAX) NULL,
        Keywords NVARCHAR(500) NULL, -- Comma-separated
        Pages INT NOT NULL DEFAULT 0,
        DownloadCount INT NOT NULL DEFAULT 0,
        ViewCount INT NOT NULL DEFAULT 0,
        Rating DECIMAL(3,2) NOT NULL DEFAULT 0.0,
        Status NVARCHAR(50) NOT NULL DEFAULT 'published', -- published, under-review, draft
        DOI NVARCHAR(200) NULL, -- Digital Object Identifier
        JournalName NVARCHAR(300) NULL,
        Volume NVARCHAR(50) NULL,
        IssueNumber NVARCHAR(50) NULL,
        PublicationDate DATE NULL,
        PdfUrl NVARCHAR(500) NULL,
        CoverImageUrl NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        IsFeatured BIT NOT NULL DEFAULT 0,
        
        INDEX IX_RESEARCH_PAPERS_Category (Category),
        INDEX IX_RESEARCH_PAPERS_Year (Year DESC),
        INDEX IX_RESEARCH_PAPERS_Institution (Institution),
        INDEX IX_RESEARCH_PAPERS_Status (Status),
        INDEX IX_RESEARCH_PAPERS_IsActive (IsActive),
        INDEX IX_RESEARCH_PAPERS_IsFeatured (IsFeatured),
        INDEX IX_RESEARCH_PAPERS_Rating (Rating DESC),
        INDEX IX_RESEARCH_PAPERS_ViewCount (ViewCount DESC)
    );
    PRINT '  ✓ RESEARCH_PAPERS table created successfully';
END
ELSE
BEGIN
    PRINT '  ℹ RESEARCH_PAPERS table already exists';
END
GO

-- =============================================
-- Step 2: Create THESIS_REPOSITORY Table
-- =============================================
PRINT 'Step 2: Creating THESIS_REPOSITORY table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'THESIS_REPOSITORY')
BEGIN
    CREATE TABLE THESIS_REPOSITORY (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(500) NOT NULL,
        Author NVARCHAR(300) NOT NULL,
        GuideNames NVARCHAR(500) NULL, -- Comma-separated thesis guides
        Institution NVARCHAR(300) NOT NULL,
        Department NVARCHAR(200) NULL,
        Year INT NOT NULL,
        Category NVARCHAR(100) NOT NULL, -- PhD Thesis, MS Thesis, MD Thesis, Post-Doctoral
        ThesisType NVARCHAR(100) NULL, -- Dissertation, Thesis, Research Project
        Abstract NVARCHAR(MAX) NOT NULL,
        ContentHtml NVARCHAR(MAX) NULL,
        Keywords NVARCHAR(500) NULL, -- Comma-separated
        Pages INT NOT NULL DEFAULT 0,
        DownloadCount INT NOT NULL DEFAULT 0,
        ViewCount INT NOT NULL DEFAULT 0,
        Rating DECIMAL(3,2) NOT NULL DEFAULT 0.0,
        Status NVARCHAR(50) NOT NULL DEFAULT 'published', -- published, under-review, draft
        SubmissionDate DATE NULL,
        ApprovalDate DATE NULL,
        DefenseDate DATE NULL,
        Grade NVARCHAR(50) NULL, -- Excellent, Very Good, Good, Pass
        PdfUrl NVARCHAR(500) NULL,
        CoverImageUrl NVARCHAR(500) NULL,
        UniversityRegistrationNumber NVARCHAR(100) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        IsFeatured BIT NOT NULL DEFAULT 0,
        
        INDEX IX_THESIS_REPOSITORY_Category (Category),
        INDEX IX_THESIS_REPOSITORY_Year (Year DESC),
        INDEX IX_THESIS_REPOSITORY_Institution (Institution),
        INDEX IX_THESIS_REPOSITORY_Status (Status),
        INDEX IX_THESIS_REPOSITORY_IsActive (IsActive),
        INDEX IX_THESIS_REPOSITORY_IsFeatured (IsFeatured),
        INDEX IX_THESIS_REPOSITORY_Rating (Rating DESC),
        INDEX IX_THESIS_REPOSITORY_ViewCount (ViewCount DESC)
    );
    PRINT '  ✓ THESIS_REPOSITORY table created successfully';
END
ELSE
BEGIN
    PRINT '  ℹ THESIS_REPOSITORY table already exists';
END
GO

-- =============================================
-- Step 3: Insert Sample Research Papers Data
-- =============================================
PRINT 'Step 3: Inserting sample research papers data...';

IF NOT EXISTS (SELECT * FROM RESEARCH_PAPERS WHERE Title = N'Efficacy of Kshara Sutra Therapy in Management of Fistula-in-Ano: A Clinical Study')
BEGIN
    INSERT INTO RESEARCH_PAPERS (
        Title, Authors, Institution, Year, Category, Abstract, Keywords,
        Pages, DownloadCount, ViewCount, Rating, Status, IsFeatured,
        JournalName, PublicationDate, CreatedAt, IsActive
    )
    VALUES 
    (
        N'Efficacy of Kshara Sutra Therapy in Management of Fistula-in-Ano: A Clinical Study',
        N'Dr. Rajesh Kumar, Dr. Priya Sharma',
        N'IPGT&RA Jamnagar',
        2024,
        N'clinical-trial',
        N'A comprehensive clinical study evaluating the effectiveness of Kshara Sutra therapy in treating anal fistula compared to conventional surgical methods. The study involved 120 patients over 18 months with significant positive outcomes.',
        N'Kshara Sutra,Fistula,Anal Surgery,Clinical Trial',
        45,
        234,
        1567,
        4.7,
        N'published',
        1,
        N'International Journal of Ayurvedic Surgery',
        '2024-03-15',
        GETUTCDATE(),
        1
    ),
    (
        N'Agnikarma in Pain Management: Ancient Technique for Modern Practice',
        N'Dr. Sameep Singh, Dr. Anil Gupta',
        N'Quadra Institute of Ayurveda',
        2024,
        N'research-paper',
        N'Exploration of Agnikarma (therapeutic cauterization) as an effective pain management technique in contemporary medical practice. Study demonstrates significant pain reduction in chronic conditions.',
        N'Agnikarma,Pain Management,Cauterization,Traditional Medicine',
        32,
        187,
        923,
        4.8,
        N'published',
        1,
        N'Journal of Traditional Medicine',
        '2024-02-20',
        GETUTCDATE(),
        1
    ),
    (
        N'Modern Integration of Sushruta''s Surgical Principles in Contemporary Operating Theaters',
        N'Dr. Saurabh Kumar, Dr. Meera Joshi',
        N'BHU Varanasi',
        2023,
        N'review-article',
        N'A comprehensive review of how ancient surgical principles from Sushruta Samhita can be integrated into modern surgical practice. Covers instrumentation, technique, and patient care.',
        N'Sushruta,Modern Surgery,Integration,Surgical Principles',
        28,
        456,
        2134,
        4.9,
        N'published',
        1,
        N'Asian Journal of Surgery',
        '2023-11-10',
        GETUTCDATE(),
        1
    ),
    (
        N'Wound Healing Properties of Traditional Ayurvedic Formulations: Laboratory Analysis',
        N'Dr. Kavita Patel, Dr. Rohit Mehta',
        N'Gujarat Ayurved University',
        2023,
        N'research-paper',
        N'Scientific analysis of wound healing properties in traditional Ayurvedic formulations used in surgical practice. Laboratory studies show enhanced healing rates.',
        N'Wound Healing,Ayurvedic Formulations,Laboratory Study,Traditional Medicine',
        38,
        312,
        1445,
        4.6,
        N'published',
        0,
        N'Journal of Ethnopharmacology',
        '2023-08-05',
        GETUTCDATE(),
        1
    ),
    (
        N'Comparative Study of Ksharasutra vs Fistulectomy in High Anal Fistula',
        N'Dr. Amit Verma, Dr. Sunita Rao',
        N'SDM College Udupi',
        2022,
        N'case-study',
        N'Comparative analysis of Ksharasutra therapy versus conventional fistulectomy in high anal fistula cases. Results show comparable efficacy with reduced complications.',
        N'Ksharasutra,Fistulectomy,Comparative Study,Anal Fistula',
        41,
        289,
        1234,
        4.5,
        N'published',
        0,
        N'Indian Journal of Surgery',
        '2022-12-15',
        GETUTCDATE(),
        1
    );
    PRINT '  ✓ Sample research papers inserted successfully';
END
ELSE
BEGIN
    PRINT '  ℹ Sample research papers already exist';
END
GO

-- =============================================
-- Step 4: Insert Sample Thesis Data
-- =============================================
PRINT 'Step 4: Inserting sample thesis data...';

IF NOT EXISTS (SELECT * FROM THESIS_REPOSITORY WHERE Title = N'AI-Assisted Diagnosis in Ayurvedic Surgery')
BEGIN
    INSERT INTO THESIS_REPOSITORY (
        Title, Author, GuideNames, Institution, Department, Year, Category,
        ThesisType, Abstract, Keywords, Pages, DownloadCount, ViewCount,
        Rating, Status, IsFeatured, SubmissionDate, ApprovalDate, Grade,
        CreatedAt, IsActive
    )
    VALUES 
    (
        N'AI-Assisted Diagnosis in Ayurvedic Surgery',
        N'Dr. Priya Sharma',
        N'Prof. R.K. Sharma, Dr. Anil Kumar',
        N'AIIMS Delhi',
        N'Department of Ayurveda',
        2024,
        N'PhD Thesis',
        N'Dissertation',
        N'This doctoral thesis explores the integration of artificial intelligence and machine learning algorithms in diagnostic processes for Ayurvedic surgical conditions. The research demonstrates significant improvements in early detection and treatment planning.',
        N'Artificial Intelligence,Machine Learning,Diagnosis,Ayurvedic Surgery,Medical Technology',
        285,
        456,
        2341,
        4.9,
        N'published',
        1,
        '2024-01-15',
        '2024-03-20',
        N'Excellent',
        GETUTCDATE(),
        1
    ),
    (
        N'Comparative Study of Ancient vs Modern Surgical Instruments',
        N'Dr. Amit Verma',
        N'Prof. S.K. Mishra',
        N'BHU Varanasi',
        N'Department of Shalya Tantra',
        2023,
        N'MS Thesis',
        N'Thesis',
        N'A comprehensive comparative analysis of surgical instruments described in Sushruta Samhita versus modern surgical tools. The study evaluates design, functionality, and clinical outcomes.',
        N'Surgical Instruments,Sushruta Samhita,Comparative Study,Medical Equipment',
        198,
        345,
        1876,
        4.7,
        N'published',
        1,
        '2023-06-10',
        '2023-08-25',
        N'Very Good',
        GETUTCDATE(),
        1
    ),
    (
        N'Evidence-Based Practice in Kshara Karma',
        N'Dr. Sunita Rao',
        N'Dr. Prakash Hegde',
        N'SDM College Udupi',
        N'Department of Shalya Tantra',
        2023,
        N'MS Thesis',
        N'Thesis',
        N'This thesis presents evidence-based protocols for Kshara Karma (caustic therapy) in various surgical conditions. Includes clinical trials and outcome analysis.',
        N'Kshara Karma,Evidence-Based Medicine,Clinical Protocols,Caustic Therapy',
        167,
        278,
        1543,
        4.6,
        N'published',
        1,
        '2023-05-20',
        '2023-07-15',
        N'Very Good',
        GETUTCDATE(),
        1
    ),
    (
        N'Role of Raktamokshana in Post-Surgical Recovery',
        N'Dr. Neha Gupta',
        N'Dr. Vijay Singh',
        N'National Institute of Ayurveda',
        N'Department of Shalya Tantra',
        2022,
        N'MD Thesis',
        N'Dissertation',
        N'Investigation of bloodletting therapy (Raktamokshana) in enhancing post-surgical recovery. Clinical study with 80 patients showing improved healing outcomes.',
        N'Raktamokshana,Bloodletting,Post-Surgical Care,Recovery,Healing',
        212,
        234,
        1298,
        4.5,
        N'published',
        0,
        '2022-11-10',
        '2023-01-20',
        N'Good',
        GETUTCDATE(),
        1
    ),
    (
        N'Minimally Invasive Techniques in Ayurvedic Surgery: A Modern Approach',
        N'Dr. Karthik Reddy',
        N'Prof. M.S. Baghel, Dr. Anita Sharma',
        N'IPGT&RA Jamnagar',
        N'Department of Shalya Tantra',
        2022,
        N'PhD Thesis',
        N'Dissertation',
        N'Exploration of minimally invasive surgical techniques based on Ayurvedic principles. Develops new protocols combining traditional wisdom with modern technology.',
        N'Minimally Invasive Surgery,Ayurveda,Modern Techniques,Surgical Innovation',
        312,
        389,
        2087,
        4.8,
        N'published',
        1,
        '2022-09-15',
        '2022-12-10',
        N'Excellent',
        GETUTCDATE(),
        1
    );
    PRINT '  ✓ Sample thesis data inserted successfully';
END
ELSE
BEGIN
    PRINT '  ℹ Sample thesis data already exists';
END
GO

-- =============================================
-- Step 5: Create Stored Procedures for Research Papers
-- =============================================
PRINT 'Step 5: Creating stored procedures for research papers...';

-- Get all research papers with filters
CREATE OR ALTER PROCEDURE sp_GetResearchPapers
    @Category NVARCHAR(100) = NULL,
    @Year INT = NULL,
    @Institution NVARCHAR(300) = NULL,
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
        Id, Title, Authors, Institution, Year, Category, Abstract,
        Keywords, Pages, DownloadCount, ViewCount, Rating, Status,
        DOI, JournalName, Volume, IssueNumber, PublicationDate,
        PdfUrl, CoverImageUrl, IsFeatured, CreatedAt, UpdatedAt
    FROM RESEARCH_PAPERS
    WHERE IsActive = 1
        AND (@Category IS NULL OR Category = @Category)
        AND (@Year IS NULL OR Year = @Year)
        AND (@Institution IS NULL OR Institution = @Institution)
        AND (@Status IS NULL OR Status = @Status)
        AND (@MinRating IS NULL OR Rating >= @MinRating)
        AND (@SearchTerm IS NULL OR 
             Title LIKE '%' + @SearchTerm + '%' OR
             Authors LIKE '%' + @SearchTerm + '%' OR
             Keywords LIKE '%' + @SearchTerm + '%' OR
             Abstract LIKE '%' + @SearchTerm + '%')
    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- Get featured research papers
CREATE OR ALTER PROCEDURE sp_GetFeaturedResearchPapers
    @Count INT = 3
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT TOP (@Count)
        Id, Title, Authors, Institution, Year, Category, Abstract,
        Keywords, Pages, DownloadCount, ViewCount, Rating, Status,
        DOI, JournalName, PublicationDate, PdfUrl, CoverImageUrl,
        IsFeatured, CreatedAt
    FROM RESEARCH_PAPERS
    WHERE IsActive = 1 AND IsFeatured = 1 AND Status = 'published'
    ORDER BY Rating DESC, ViewCount DESC;
END
GO

-- Get research paper by ID with content
CREATE OR ALTER PROCEDURE sp_GetResearchPaperById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM RESEARCH_PAPERS
    WHERE Id = @Id AND IsActive = 1;
    
    -- Increment view count
    UPDATE RESEARCH_PAPERS
    SET ViewCount = ViewCount + 1,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;
END
GO

-- =============================================
-- Step 6: Create Stored Procedures for Thesis
-- =============================================
PRINT 'Step 6: Creating stored procedures for thesis...';

-- Get all thesis with filters
CREATE OR ALTER PROCEDURE sp_GetThesisRepository
    @Category NVARCHAR(100) = NULL,
    @Year INT = NULL,
    @Institution NVARCHAR(300) = NULL,
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
        Id, Title, Author, GuideNames, Institution, Department, Year,
        Category, ThesisType, Abstract, Keywords, Pages, DownloadCount,
        ViewCount, Rating, Status, SubmissionDate, ApprovalDate,
        DefenseDate, Grade, PdfUrl, CoverImageUrl, IsFeatured,
        UniversityRegistrationNumber, CreatedAt, UpdatedAt
    FROM THESIS_REPOSITORY
    WHERE IsActive = 1
        AND (@Category IS NULL OR Category = @Category)
        AND (@Year IS NULL OR Year = @Year)
        AND (@Institution IS NULL OR Institution = @Institution)
        AND (@Status IS NULL OR Status = @Status)
        AND (@MinRating IS NULL OR Rating >= @MinRating)
        AND (@SearchTerm IS NULL OR 
             Title LIKE '%' + @SearchTerm + '%' OR
             Author LIKE '%' + @SearchTerm + '%' OR
             Keywords LIKE '%' + @SearchTerm + '%' OR
             Abstract LIKE '%' + @SearchTerm + '%')
    ORDER BY IsFeatured DESC, ViewCount DESC, Rating DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- Get featured thesis
CREATE OR ALTER PROCEDURE sp_GetFeaturedThesis
    @Count INT = 3
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT TOP (@Count)
        Id, Title, Author, GuideNames, Institution, Department, Year,
        Category, ThesisType, Abstract, Keywords, Pages, DownloadCount,
        ViewCount, Rating, Status, Grade, PdfUrl, CoverImageUrl,
        IsFeatured, CreatedAt
    FROM THESIS_REPOSITORY
    WHERE IsActive = 1 AND IsFeatured = 1 AND Status = 'published'
    ORDER BY Rating DESC, ViewCount DESC;
END
GO

-- Get thesis by ID with content
CREATE OR ALTER PROCEDURE sp_GetThesisById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM THESIS_REPOSITORY
    WHERE Id = @Id AND IsActive = 1;
    
    -- Increment view count
    UPDATE THESIS_REPOSITORY
    SET ViewCount = ViewCount + 1,
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;
END
GO

-- =============================================
-- Step 7: Create Statistics Views
-- =============================================
PRINT 'Step 7: Creating statistics views...';

CREATE OR ALTER VIEW vw_ResearchStatistics AS
SELECT 
    COUNT(*) AS TotalPapers,
    SUM(CASE WHEN IsFeatured = 1 THEN 1 ELSE 0 END) AS FeaturedPapers,
    SUM(ViewCount) AS TotalViews,
    SUM(DownloadCount) AS TotalDownloads,
    AVG(Rating) AS AverageRating,
    COUNT(DISTINCT Institution) AS TotalInstitutions,
    COUNT(DISTINCT Year) AS YearsSpanned
FROM RESEARCH_PAPERS
WHERE IsActive = 1 AND Status = 'published';
GO

CREATE OR ALTER VIEW vw_ThesisStatistics AS
SELECT 
    COUNT(*) AS TotalThesis,
    SUM(CASE WHEN IsFeatured = 1 THEN 1 ELSE 0 END) AS FeaturedThesis,
    SUM(ViewCount) AS TotalViews,
    SUM(DownloadCount) AS TotalDownloads,
    AVG(Rating) AS AverageRating,
    COUNT(DISTINCT Institution) AS TotalInstitutions,
    COUNT(DISTINCT Year) AS YearsSpanned
FROM THESIS_REPOSITORY
WHERE IsActive = 1 AND Status = 'published';
GO

PRINT '========================================';
PRINT 'Research & Thesis Repository Schema Created Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Summary:';
PRINT '  ✓ RESEARCH_PAPERS table created';
PRINT '  ✓ THESIS_REPOSITORY table created';
PRINT '  ✓ Sample data inserted';
PRINT '  ✓ Stored procedures created';
PRINT '  ✓ Statistics views created';
PRINT '';
PRINT 'Next Steps:';
PRINT '  1. Verify data: SELECT * FROM RESEARCH_PAPERS';
PRINT '  2. Verify data: SELECT * FROM THESIS_REPOSITORY';
PRINT '  3. Check stats: SELECT * FROM vw_ResearchStatistics';
PRINT '  4. Check stats: SELECT * FROM vw_ThesisStatistics';
GO


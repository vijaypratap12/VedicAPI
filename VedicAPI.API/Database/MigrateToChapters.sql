-- =============================================
-- Vedic API Database Migration
-- Migration: Convert to Chapter-Based Book System
-- Description: Alter Books table and create BookChapters table
-- =============================================

USE VedicDB;
GO

PRINT 'Starting migration to chapter-based book system...';
GO

-- =============================================
-- Step 1: Alter Books Table
-- =============================================
PRINT 'Step 1: Altering Books table...';

-- Drop the Content column
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BOOKS') AND name = 'Content')
BEGIN
    ALTER TABLE BOOKS DROP COLUMN Content;
    PRINT '  - Dropped Content column';
END

-- Add TotalChapters column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BOOKS') AND name = 'TotalChapters')
BEGIN
    ALTER TABLE BOOKS ADD TotalChapters INT NOT NULL DEFAULT 0;
    PRINT '  - Added TotalChapters column';
END

-- Add CoverImageUrl column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('BOOKS') AND name = 'CoverImageUrl')
BEGIN
    ALTER TABLE BOOKS ADD CoverImageUrl NVARCHAR(500) NULL;
    PRINT '  - Added CoverImageUrl column';
END

PRINT 'Books table altered successfully!';
GO

-- =============================================
-- Step 2: Create BookChapters Table
-- =============================================
PRINT 'Step 2: Creating BookChapters table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BOOKCHAPTERS')
BEGIN
    CREATE TABLE BOOKCHAPTERS (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BookId INT NOT NULL,
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
        CONSTRAINT FK_BookChapters_Books FOREIGN KEY (BookId) 
            REFERENCES BOOKS(Id) ON DELETE CASCADE,
        
        -- Unique constraint
        CONSTRAINT UQ_BookChapter UNIQUE (BookId, ChapterNumber)
    );
    
    -- Create Indexes
    CREATE INDEX IX_BookChapters_BookId ON BOOKCHAPTERS(BookId);
    CREATE INDEX IX_BookChapters_DisplayOrder ON BOOKCHAPTERS(BookId, DisplayOrder);
    CREATE INDEX IX_BookChapters_ChapterNumber ON BOOKCHAPTERS(BookId, ChapterNumber);
    
    PRINT 'BookChapters table created successfully!';
END
ELSE
BEGIN
    PRINT 'BookChapters table already exists';
END
GO

-- =============================================
-- Step 3: Insert Sample Chapter Data
-- =============================================
PRINT 'Step 3: Inserting sample chapter data...';

-- Get BookIds
DECLARE @SushrutaId INT = (SELECT Id FROM BOOKS WHERE Title LIKE '%Sushruta Samhita%');
DECLARE @CharakaId INT = (SELECT Id FROM BOOKS WHERE Title LIKE '%Charaka Samhita%');
DECLARE @AshtangaId INT = (SELECT Id FROM BOOKS WHERE Title LIKE '%Ashtanga Hridaya%');

-- Insert chapters for Sushruta Samhita
IF @SushrutaId IS NOT NULL AND NOT EXISTS (SELECT * FROM BOOKCHAPTERS WHERE BookId = @SushrutaId)
BEGIN
    INSERT INTO BOOKCHAPTERS (BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, ContentHtml, Summary, WordCount, ReadingTimeMinutes, DisplayOrder, CreatedAt, IsActive)
    VALUES 
    (
        @SushrutaId,
        1,
        'Introduction to Shalya Tantra',
        'The Ancient Science of Surgery',
        '<div class="chapter-content">
            <h2>Historical Context</h2>
            <p>The <strong>Sushruta Samhita</strong> is one of the most important ancient texts on medicine and surgery. It describes over 300 surgical procedures and 120 surgical instruments.</p>
            
            <h3>The Eight Branches of Ayurveda</h3>
            <p>The text is divided into six sections:</p>
            <ul>
                <li><strong>Sutrasthana</strong> - General principles</li>
                <li><strong>Nidanasthana</strong> - Pathology</li>
                <li><strong>Sharirasthana</strong> - Anatomy</li>
                <li><strong>Chikitsasthana</strong> - Therapeutics</li>
                <li><strong>Kalpasthana</strong> - Toxicology</li>
                <li><strong>Uttaratantra</strong> - Specialized treatments</li>
            </ul>
            
            <blockquote>
                <p>"योगः कर्मसु कौशलम्" (Yogaḥ karmasu kauśalam)</p>
                <footer>Excellence in action is yoga. This principle guides the surgeon''s hand.</footer>
            </blockquote>
            
            <h3>Fundamental Principles</h3>
            <p>The practice of Shalya Tantra is built upon several fundamental principles:</p>
            <ol>
                <li><strong>Yantra Karma</strong> - Use of instruments</li>
                <li><strong>Shastra Karma</strong> - Sharp instrument procedures</li>
                <li><strong>Agni Karma</strong> - Thermal cauterization</li>
                <li><strong>Kshara Karma</strong> - Alkaline cauterization</li>
                <li><strong>Jalauka Avacharana</strong> - Leech therapy</li>
            </ol>
        </div>',
        'Introduction to the ancient science of surgery in Ayurveda, covering historical context and fundamental principles.',
        450,
        5,
        1,
        GETUTCDATE(),
        1
    ),
    (
        @SushrutaId,
        2,
        'Surgical Instruments',
        'Yantra Shastra - The 101 Instruments',
        '<div class="chapter-content">
            <h2>Classification of Surgical Instruments</h2>
            <p>Maharishi Sushruta classified surgical instruments into <strong>101 types</strong>, categorized based on their function and design.</p>
            
            <h3>Main Categories</h3>
            <h4>1. Swastika (Blunt Instruments)</h4>
            <p>These instruments are used for probing, measuring, and manipulating tissues without cutting.</p>
            <ul>
                <li>Shalaka - Probes</li>
                <li>Vriddhipatra - Spatulas</li>
                <li>Tala Yantra - Clamps</li>
            </ul>
            
            <h4>2. Shastra (Sharp Instruments)</h4>
            <p>Sharp instruments used for incision and excision:</p>
            <ul>
                <li>Mandalagra - Circular knives</li>
                <li>Vrihimukha - Scalpels</li>
                <li>Kartari - Scissors</li>
            </ul>
            
            <h3>Material and Construction</h3>
            <p>The instruments were traditionally made from:</p>
            <ul>
                <li>Steel (Loha)</li>
                <li>Stone (Ashma)</li>
                <li>Wood (Kashtha)</li>
                <li>Bamboo (Venu)</li>
            </ul>
            
            <blockquote>
                <p>The surgeon must be skilled in the use of all instruments, as a musician is skilled with all notes.</p>
            </blockquote>
        </div>',
        'Detailed description of the 101 surgical instruments classified by Sushruta, their uses and construction.',
        380,
        4,
        2,
        GETUTCDATE(),
        1
    ),
    (
        @SushrutaId,
        3,
        'Surgical Procedures',
        'Ancient Techniques Still Relevant Today',
        '<div class="chapter-content">
            <h2>Overview of Surgical Procedures</h2>
            <p>The Sushruta Samhita describes over <strong>300 surgical procedures</strong>, many of which form the basis of modern surgical techniques.</p>
            
            <h3>Major Surgical Procedures</h3>
            
            <h4>Rhinoplasty (Nasikasandhana)</h4>
            <p>Perhaps the most famous procedure, the reconstruction of the nose using a forehead flap technique. This procedure is still used in modern plastic surgery.</p>
            
            <h4>Cataract Surgery (Linganasha)</h4>
            <p>Sushruta described a technique called "couching" for treating cataracts, where a curved needle was used to push the lens down.</p>
            
            <h4>Lithotomy (Ashmari Bheda)</h4>
            <p>Surgical removal of bladder stones through a perineal approach.</p>
            
            <h3>Pre-operative Preparation</h3>
            <ol>
                <li><strong>Patient Assessment</strong> - Evaluation of constitution (Prakriti)</li>
                <li><strong>Purification</strong> - Panchakarma procedures</li>
                <li><strong>Timing</strong> - Consideration of seasons and auspicious times</li>
                <li><strong>Instruments</strong> - Sterilization and preparation</li>
            </ol>
            
            <h3>Post-operative Care</h3>
            <p>Sushruta emphasized the importance of post-operative care:</p>
            <ul>
                <li>Wound dressing with medicated oils</li>
                <li>Dietary restrictions</li>
                <li>Rest and recuperation</li>
                <li>Prevention of infection</li>
            </ul>
        </div>',
        'Comprehensive overview of surgical procedures described in Sushruta Samhita, including rhinoplasty, cataract surgery, and lithotomy.',
        420,
        5,
        3,
        GETUTCDATE(),
        1
    );
    
    -- Update total chapters for Sushruta Samhita
    UPDATE BOOKS SET TotalChapters = 3 WHERE Id = @SushrutaId;
    
    PRINT '  - Added 3 chapters for Sushruta Samhita';
END

-- Insert chapters for Charaka Samhita
IF @CharakaId IS NOT NULL AND NOT EXISTS (SELECT * FROM BOOKCHAPTERS WHERE BookId = @CharakaId)
BEGIN
    INSERT INTO BOOKCHAPTERS (BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, ContentHtml, Summary, WordCount, ReadingTimeMinutes, DisplayOrder, CreatedAt, IsActive)
    VALUES 
    (
        @CharakaId,
        1,
        'Principles of Ayurvedic Medicine',
        'Foundation of Internal Medicine',
        '<div class="chapter-content">
            <h2>Introduction to Charaka Samhita</h2>
            <p>The <strong>Charaka Samhita</strong> is one of the two foundational texts of Ayurveda, emphasizing internal medicine and the importance of digestion, metabolism, and immunity.</p>
            
            <h3>The Three Doshas</h3>
            <p>Ayurveda is based on the concept of three fundamental energies:</p>
            <ul>
                <li><strong>Vata</strong> - The principle of movement (Air + Ether)</li>
                <li><strong>Pitta</strong> - The principle of transformation (Fire + Water)</li>
                <li><strong>Kapha</strong> - The principle of structure (Water + Earth)</li>
            </ul>
            
            <h3>Health and Disease</h3>
            <p>Health is defined as the balanced state of doshas, dhatus (tissues), malas (waste products), and agni (digestive fire), along with clarity of senses, mind, and soul.</p>
            
            <blockquote>
                <p>Health is the foundation of dharma, artha, kama, and moksha - the four pursuits of life.</p>
            </blockquote>
        </div>',
        'Introduction to the fundamental principles of Ayurvedic medicine as described in Charaka Samhita.',
        320,
        4,
        1,
        GETUTCDATE(),
        1
    ),
    (
        @CharakaId,
        2,
        'Diagnosis and Treatment',
        'The Art of Healing',
        '<div class="chapter-content">
            <h2>Diagnostic Methods</h2>
            <p>Charaka described eight methods of examination:</p>
            
            <h3>Ashtavidha Pariksha (Eight-fold Examination)</h3>
            <ol>
                <li><strong>Nadi</strong> - Pulse examination</li>
                <li><strong>Mutra</strong> - Urine examination</li>
                <li><strong>Mala</strong> - Stool examination</li>
                <li><strong>Jihva</strong> - Tongue examination</li>
                <li><strong>Shabda</strong> - Voice and sound</li>
                <li><strong>Sparsha</strong> - Touch and palpation</li>
                <li><strong>Drik</strong> - Visual observation</li>
                <li><strong>Akriti</strong> - General appearance</li>
            </ol>
            
            <h3>Treatment Principles</h3>
            <p>Treatment in Ayurveda follows a systematic approach:</p>
            <ul>
                <li><strong>Nidana Parivarjana</strong> - Avoidance of causative factors</li>
                <li><strong>Shodhan</strong> - Purification therapies</li>
                <li><strong>Shaman</strong> - Palliative treatments</li>
                <li><strong>Rasayana</strong> - Rejuvenation</li>
            </ul>
        </div>',
        'Detailed explanation of diagnostic methods and treatment principles in Ayurvedic medicine.',
        280,
        3,
        2,
        GETUTCDATE(),
        1
    );
    
    -- Update total chapters for Charaka Samhita
    UPDATE BOOKS SET TotalChapters = 2 WHERE Id = @CharakaId;
    
    PRINT '  - Added 2 chapters for Charaka Samhita';
END

-- Insert chapters for Ashtanga Hridaya
IF @AshtangaId IS NOT NULL AND NOT EXISTS (SELECT * FROM BOOKCHAPTERS WHERE BookId = @AshtangaId)
BEGIN
    INSERT INTO BOOKCHAPTERS (BookId, ChapterNumber, ChapterTitle, ChapterSubtitle, ContentHtml, Summary, WordCount, ReadingTimeMinutes, DisplayOrder, CreatedAt, IsActive)
    VALUES 
    (
        @AshtangaId,
        1,
        'Synthesis of Ayurvedic Knowledge',
        'Combining Charaka and Sushruta',
        '<div class="chapter-content">
            <h2>Introduction to Ashtanga Hridaya</h2>
            <p>The <strong>Ashtanga Hridaya</strong> is a comprehensive medical text that synthesizes knowledge from both Charaka Samhita and Sushruta Samhita, written by Vagbhata.</p>
            
            <h3>The Eight Branches</h3>
            <p>Ashtanga means "eight branches," referring to:</p>
            <ol>
                <li><strong>Kaya Chikitsa</strong> - Internal medicine</li>
                <li><strong>Shalya Tantra</strong> - Surgery</li>
                <li><strong>Shalakya Tantra</strong> - ENT and ophthalmology</li>
                <li><strong>Kaumarabhritya</strong> - Pediatrics</li>
                <li><strong>Agada Tantra</strong> - Toxicology</li>
                <li><strong>Bhuta Vidya</strong> - Psychiatry</li>
                <li><strong>Rasayana</strong> - Rejuvenation</li>
                <li><strong>Vajikarana</strong> - Aphrodisiacs and reproductive health</li>
            </ol>
            
            <h3>Unique Contributions</h3>
            <p>Vagbhata made several unique contributions:</p>
            <ul>
                <li>Simplified and systematized the knowledge</li>
                <li>Written in verse form for easy memorization</li>
                <li>Added practical insights from clinical experience</li>
                <li>Emphasized preventive medicine</li>
            </ul>
            
            <blockquote>
                <p>Prevention is better than cure - maintain health through proper diet, lifestyle, and seasonal routines.</p>
            </blockquote>
        </div>',
        'Overview of Ashtanga Hridaya, its eight branches, and unique contributions to Ayurvedic medicine.',
        350,
        4,
        1,
        GETUTCDATE(),
        1
    );
    
    -- Update total chapters for Ashtanga Hridaya
    UPDATE BOOKS SET TotalChapters = 1 WHERE Id = @AshtangaId;
    
    PRINT '  - Added 1 chapter for Ashtanga Hridaya';
END

GO

PRINT '';
PRINT '=============================================';
PRINT 'Migration completed successfully!';
PRINT '=============================================';
PRINT '';
PRINT 'Summary:';
PRINT '  - Books table altered (Content dropped, TotalChapters and CoverImageUrl added)';
PRINT '  - BookChapters table created with indexes';
PRINT '  - Sample chapter data inserted';
PRINT '';
PRINT 'Next steps:';
PRINT '  1. Update backend models and DTOs';
PRINT '  2. Implement chapter repository and service';
PRINT '  3. Add chapter API endpoints';
PRINT '  4. Update frontend to use chapter-based system';
GO


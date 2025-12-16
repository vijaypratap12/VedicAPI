-- =============================================
-- Create Contact and Newsletter Tables
-- =============================================

USE VedicSurgeryDB;
GO

-- =============================================
-- 1. CONTACTSUBMISSIONS Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CONTACTSUBMISSIONS')
BEGIN
    CREATE TABLE CONTACTSUBMISSIONS (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(200) NOT NULL,
        Email NVARCHAR(255) NOT NULL,
        Organization NVARCHAR(300) NULL,
        Subject NVARCHAR(500) NOT NULL,
        Message NVARCHAR(MAX) NOT NULL,
        ContactType NVARCHAR(50) NOT NULL, -- general, collaboration, contribution, technical, research, feedback
        Status NVARCHAR(50) DEFAULT 'Pending', -- Pending, InProgress, Resolved, Closed
        Priority NVARCHAR(20) DEFAULT 'Normal', -- Low, Normal, High, Urgent
        SubmittedAt DATETIME DEFAULT GETDATE(),
        RespondedAt DATETIME NULL,
        RespondedBy NVARCHAR(200) NULL,
        ResponseMessage NVARCHAR(MAX) NULL,
        Notes NVARCHAR(MAX) NULL,
        IsArchived BIT DEFAULT 0,
        IpAddress NVARCHAR(50) NULL,
        UserAgent NVARCHAR(500) NULL,
        CreatedAt DATETIME DEFAULT GETDATE(),
        UpdatedAt DATETIME DEFAULT GETDATE()
    );

    -- Create indexes for better performance
    CREATE INDEX IX_CONTACTSUBMISSIONS_Email ON CONTACTSUBMISSIONS(Email);
    CREATE INDEX IX_CONTACTSUBMISSIONS_Status ON CONTACTSUBMISSIONS(Status);
    CREATE INDEX IX_CONTACTSUBMISSIONS_ContactType ON CONTACTSUBMISSIONS(ContactType);
    CREATE INDEX IX_CONTACTSUBMISSIONS_SubmittedAt ON CONTACTSUBMISSIONS(SubmittedAt DESC);

    PRINT 'CONTACTSUBMISSIONS table created successfully';
END
ELSE
BEGIN
    PRINT 'CONTACTSUBMISSIONS table already exists';
END
GO

-- =============================================
-- 2. NEWSLETTERSUBSCRIPTIONS Table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NEWSLETTERSUBSCRIPTIONS')
BEGIN
    CREATE TABLE NEWSLETTERSUBSCRIPTIONS (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Email NVARCHAR(255) UNIQUE NOT NULL,
        SubscribedAt DATETIME DEFAULT GETDATE(),
        IsActive BIT DEFAULT 1,
        UnsubscribeToken UNIQUEIDENTIFIER DEFAULT NEWID(),
        UnsubscribedAt DATETIME NULL,
        Source NVARCHAR(100) DEFAULT 'Contact Page',
        IpAddress NVARCHAR(50) NULL,
        ConfirmationSent BIT DEFAULT 0,
        ConfirmationSentAt DATETIME NULL,
        EmailsSentCount INT DEFAULT 0,
        LastEmailSentAt DATETIME NULL,
        CreatedAt DATETIME DEFAULT GETDATE(),
        UpdatedAt DATETIME DEFAULT GETDATE()
    );

    -- Create indexes
    CREATE INDEX IX_NEWSLETTERSUBSCRIPTIONS_Email ON NEWSLETTERSUBSCRIPTIONS(Email);
    CREATE INDEX IX_NEWSLETTERSUBSCRIPTIONS_IsActive ON NEWSLETTERSUBSCRIPTIONS(IsActive);
    CREATE INDEX IX_NEWSLETTERSUBSCRIPTIONS_SubscribedAt ON NEWSLETTERSUBSCRIPTIONS(SubscribedAt DESC);

    PRINT 'NEWSLETTERSUBSCRIPTIONS table created successfully';
END
ELSE
BEGIN
    PRINT 'NEWSLETTERSUBSCRIPTIONS table already exists';
END
GO

-- =============================================
-- 3. Insert Sample Data (Optional - for testing)
-- =============================================
-- Uncomment below to insert sample data

/*
-- Sample Contact Submissions
INSERT INTO CONTACTSUBMISSIONS (Name, Email, Organization, Subject, Message, ContactType, Status)
VALUES 
    ('Dr. Rajesh Kumar', 'rajesh@example.com', 'AIIMS Delhi', 'Research Collaboration', 'I am interested in collaborating on Ayurvedic surgery research.', 'collaboration', 'Pending'),
    ('Priya Sharma', 'priya@example.com', NULL, 'Platform Feedback', 'Great platform! Would love to see more content on surgical techniques.', 'feedback', 'Resolved'),
    ('Dr. Amit Patel', 'amit@example.com', 'BHU Varanasi', 'Technical Issue', 'Unable to access some research papers.', 'technical', 'InProgress');

-- Sample Newsletter Subscriptions
INSERT INTO NEWSLETTERSUBSCRIPTIONS (Email, Source)
VALUES 
    ('subscriber1@example.com', 'Contact Page'),
    ('subscriber2@example.com', 'Home Page'),
    ('subscriber3@example.com', 'Research Page');

PRINT 'Sample data inserted successfully';
*/

-- =============================================
-- 4. Create Stored Procedures (Optional)
-- =============================================

-- Procedure to get contact submissions by status
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'GetContactSubmissionsByStatus')
    DROP PROCEDURE GetContactSubmissionsByStatus;
GO

CREATE PROCEDURE GetContactSubmissionsByStatus
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id, Name, Email, Organization, Subject, Message, 
        ContactType, Status, Priority, SubmittedAt, RespondedAt,
        RespondedBy, IsArchived
    FROM CONTACTSUBMISSIONS
    WHERE Status = @Status AND IsArchived = 0
    ORDER BY SubmittedAt DESC;
END
GO

-- Procedure to get active newsletter subscribers
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'GetActiveNewsletterSubscribers')
    DROP PROCEDURE GetActiveNewsletterSubscribers;
GO

CREATE PROCEDURE GetActiveNewsletterSubscribers
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id, Email, SubscribedAt, Source, EmailsSentCount, LastEmailSentAt
    FROM NEWSLETTERSUBSCRIPTIONS
    WHERE IsActive = 1
    ORDER BY SubscribedAt DESC;
END
GO

PRINT 'Database setup completed successfully!';
GO


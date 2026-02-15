-- =============================================
-- Vedic API Database - Treatment Recommendation System
-- Description: Create tables for AYUSH Treatment Recommendation System
-- Date: 2026-02-15
-- =============================================

USE VedicDB;
GO

PRINT '========================================';
PRINT 'Creating Treatment Recommendation System Tables';
PRINT '========================================';
PRINT '';

-- =============================================
-- Table 1: PATIENTS
-- Description: Core patient information table
-- =============================================
PRINT 'Step 1/10: Creating PATIENTS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PATIENTS')
BEGIN
    CREATE TABLE PATIENTS (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NULL,  -- FK to USERS table (nullable for walk-in patients)
        Name NVARCHAR(200) NOT NULL,
        Age INT NOT NULL,
        Gender NVARCHAR(10) NOT NULL,
        ContactNumber NVARCHAR(15) NULL,
        Email NVARCHAR(200) NULL,
        Address NVARCHAR(500) NULL,
        Prakriti NVARCHAR(50) NULL,  -- Vata/Pitta/Kapha/Vata-Pitta/etc
        PrakritiScore NVARCHAR(100) NULL,  -- JSON: {"vata":30,"pitta":45,"kapha":25}
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        
        CONSTRAINT FK_Patients_Users FOREIGN KEY (UserId) REFERENCES USERS(Id)
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_Patients_UserId ON PATIENTS(UserId);
    CREATE NONCLUSTERED INDEX IX_Patients_Prakriti ON PATIENTS(Prakriti);
    CREATE NONCLUSTERED INDEX IX_Patients_IsActive ON PATIENTS(IsActive) WHERE IsActive = 1;
    CREATE NONCLUSTERED INDEX IX_Patients_Email ON PATIENTS(Email);
    
    PRINT '  ✓ PATIENTS table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ PATIENTS table already exists';
END
GO

-- =============================================
-- Table 2: PRAKRITIQUESTIONS
-- Description: Standardized Prakriti assessment questionnaire
-- =============================================
PRINT 'Step 2/10: Creating PRAKRITIQUESTIONS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PRAKRITIQUESTIONS')
BEGIN
    CREATE TABLE PRAKRITIQUESTIONS (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Category NVARCHAR(100) NOT NULL,  -- Physical/Mental/Digestive/Sleep
        Question NVARCHAR(500) NOT NULL,
        VataOption NVARCHAR(300) NOT NULL,
        PittaOption NVARCHAR(300) NOT NULL,
        KaphaOption NVARCHAR(300) NOT NULL,
        DisplayOrder INT NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_PrakritiQuestions_Category ON PRAKRITIQUESTIONS(Category);
    CREATE NONCLUSTERED INDEX IX_PrakritiQuestions_DisplayOrder ON PRAKRITIQUESTIONS(DisplayOrder);
    CREATE NONCLUSTERED INDEX IX_PrakritiQuestions_IsActive ON PRAKRITIQUESTIONS(IsActive) WHERE IsActive = 1;
    
    PRINT '  ✓ PRAKRITIQUESTIONS table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ PRAKRITIQUESTIONS table already exists';
END
GO

-- =============================================
-- Table 3: PRAKRITIASSESSMENTS
-- Description: Store patient assessment responses
-- =============================================
PRINT 'Step 3/10: Creating PRAKRITIASSESSMENTS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PRAKRITIASSESSMENTS')
BEGIN
    CREATE TABLE PRAKRITIASSESSMENTS (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        PatientId BIGINT NOT NULL,
        Responses NVARCHAR(MAX) NOT NULL,  -- JSON array of question-answer pairs
        VataScore INT NOT NULL,
        PittaScore INT NOT NULL,
        KaphaScore INT NOT NULL,
        DominantPrakriti NVARCHAR(50) NOT NULL,
        AssessedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        
        CONSTRAINT FK_PrakritiAssessments_Patients FOREIGN KEY (PatientId) 
            REFERENCES PATIENTS(Id) ON DELETE CASCADE
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_PrakritiAssessments_PatientId ON PRAKRITIASSESSMENTS(PatientId);
    CREATE NONCLUSTERED INDEX IX_PrakritiAssessments_DominantPrakriti ON PRAKRITIASSESSMENTS(DominantPrakriti);
    CREATE NONCLUSTERED INDEX IX_PrakritiAssessments_AssessedAt ON PRAKRITIASSESSMENTS(AssessedAt DESC);
    
    PRINT '  ✓ PRAKRITIASSESSMENTS table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ PRAKRITIASSESSMENTS table already exists';
END
GO

-- =============================================
-- Table 4: CONDITIONS
-- Description: Medical conditions/diseases database
-- =============================================
PRINT 'Step 4/10: Creating CONDITIONS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CONDITIONS')
BEGIN
    CREATE TABLE CONDITIONS (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        SanskritName NVARCHAR(200) NULL,
        Category NVARCHAR(100) NOT NULL,  -- Chronic/Acute/Lifestyle
        Description NVARCHAR(MAX) NULL,
        CommonSymptoms NVARCHAR(MAX) NULL,
        AffectedDoshas NVARCHAR(100) NULL,  -- Vata/Pitta/Kapha combinations
        Severity NVARCHAR(50) NULL,  -- Mild/Moderate/Severe
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_Conditions_Category ON CONDITIONS(Category);
    CREATE NONCLUSTERED INDEX IX_Conditions_Name ON CONDITIONS(Name);
    CREATE NONCLUSTERED INDEX IX_Conditions_IsActive ON CONDITIONS(IsActive) WHERE IsActive = 1;
    CREATE NONCLUSTERED INDEX IX_Conditions_Category_IsActive ON CONDITIONS(Category, IsActive);
    
    PRINT '  ✓ CONDITIONS table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ CONDITIONS table already exists';
END
GO

-- =============================================
-- Table 5: HERBALMEDICINES
-- Description: Comprehensive herbal medicine database
-- =============================================
PRINT 'Step 5/10: Creating HERBALMEDICINES table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'HERBALMEDICINES')
BEGIN
    CREATE TABLE HERBALMEDICINES (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        CommonName NVARCHAR(200) NOT NULL,
        SanskritName NVARCHAR(200) NULL,
        ScientificName NVARCHAR(200) NULL,
        HindiName NVARCHAR(200) NULL,
        Properties NVARCHAR(MAX) NULL,  -- Rasa, Guna, Virya, Vipaka
        Indications NVARCHAR(MAX) NULL,
        Dosage NVARCHAR(500) NULL,
        Contraindications NVARCHAR(MAX) NULL,
        SideEffects NVARCHAR(MAX) NULL,
        ImageUrl NVARCHAR(500) NULL,
        VataEffect NVARCHAR(50) NULL,  -- Balancing/Aggravating/Neutral
        PittaEffect NVARCHAR(50) NULL,
        KaphaEffect NVARCHAR(50) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_HerbalMedicines_CommonName ON HERBALMEDICINES(CommonName);
    CREATE NONCLUSTERED INDEX IX_HerbalMedicines_SanskritName ON HERBALMEDICINES(SanskritName);
    CREATE NONCLUSTERED INDEX IX_HerbalMedicines_IsActive ON HERBALMEDICINES(IsActive) WHERE IsActive = 1;
    CREATE NONCLUSTERED INDEX IX_HerbalMedicines_VataEffect ON HERBALMEDICINES(VataEffect);
    CREATE NONCLUSTERED INDEX IX_HerbalMedicines_PittaEffect ON HERBALMEDICINES(PittaEffect);
    CREATE NONCLUSTERED INDEX IX_HerbalMedicines_KaphaEffect ON HERBALMEDICINES(KaphaEffect);
    
    PRINT '  ✓ HERBALMEDICINES table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ HERBALMEDICINES table already exists';
END
GO

-- =============================================
-- Table 6: YOGAASANAS
-- Description: Yoga postures and breathing exercises
-- =============================================
PRINT 'Step 6/10: Creating YOGAASANAS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'YOGAASANAS')
BEGIN
    CREATE TABLE YOGAASANAS (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        AsanaName NVARCHAR(200) NOT NULL,
        SanskritName NVARCHAR(200) NULL,
        Category NVARCHAR(100) NOT NULL,  -- Standing/Sitting/Lying/Pranayama
        Benefits NVARCHAR(MAX) NULL,
        Duration NVARCHAR(100) NULL,
        Difficulty NVARCHAR(50) NULL,  -- Beginner/Intermediate/Advanced
        Instructions NVARCHAR(MAX) NULL,
        Precautions NVARCHAR(MAX) NULL,
        ImageUrl NVARCHAR(500) NULL,
        VideoUrl NVARCHAR(500) NULL,
        VataEffect NVARCHAR(50) NULL,
        PittaEffect NVARCHAR(50) NULL,
        KaphaEffect NVARCHAR(50) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_YogaAsanas_Category ON YOGAASANAS(Category);
    CREATE NONCLUSTERED INDEX IX_YogaAsanas_Difficulty ON YOGAASANAS(Difficulty);
    CREATE NONCLUSTERED INDEX IX_YogaAsanas_IsActive ON YOGAASANAS(IsActive) WHERE IsActive = 1;
    CREATE NONCLUSTERED INDEX IX_YogaAsanas_AsanaName ON YOGAASANAS(AsanaName);
    
    PRINT '  ✓ YOGAASANAS table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ YOGAASANAS table already exists';
END
GO

-- =============================================
-- Table 7: DIETARYITEMS
-- Description: Food items with Ayurvedic properties
-- =============================================
PRINT 'Step 7/10: Creating DIETARYITEMS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DIETARYITEMS')
BEGIN
    CREATE TABLE DIETARYITEMS (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        FoodName NVARCHAR(200) NOT NULL,
        Category NVARCHAR(100) NOT NULL,  -- Grains/Vegetables/Fruits/Spices/Dairy/Proteins
        VataEffect NVARCHAR(50) NULL,
        PittaEffect NVARCHAR(50) NULL,
        KaphaEffect NVARCHAR(50) NULL,
        Properties NVARCHAR(MAX) NULL,
        Benefits NVARCHAR(MAX) NULL,
        Rasa NVARCHAR(100) NULL,  -- Taste: Sweet/Sour/Salty/Bitter/Pungent/Astringent
        Virya NVARCHAR(50) NULL,  -- Heating/Cooling
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_DietaryItems_Category ON DIETARYITEMS(Category);
    CREATE NONCLUSTERED INDEX IX_DietaryItems_IsActive ON DIETARYITEMS(IsActive) WHERE IsActive = 1;
    CREATE NONCLUSTERED INDEX IX_DietaryItems_FoodName ON DIETARYITEMS(FoodName);
    CREATE NONCLUSTERED INDEX IX_DietaryItems_VataEffect ON DIETARYITEMS(VataEffect);
    CREATE NONCLUSTERED INDEX IX_DietaryItems_PittaEffect ON DIETARYITEMS(PittaEffect);
    CREATE NONCLUSTERED INDEX IX_DietaryItems_KaphaEffect ON DIETARYITEMS(KaphaEffect);
    
    PRINT '  ✓ DIETARYITEMS table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ DIETARYITEMS table already exists';
END
GO

-- =============================================
-- Table 8: TREATMENTPLANS
-- Description: Generated treatment recommendations
-- =============================================
PRINT 'Step 8/10: Creating TREATMENTPLANS table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TREATMENTPLANS')
BEGIN
    CREATE TABLE TREATMENTPLANS (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        PatientId BIGINT NOT NULL,
        ConditionId BIGINT NOT NULL,
        Prakriti NVARCHAR(50) NOT NULL,
        HerbalMedicines NVARCHAR(MAX) NULL,  -- JSON array of medicine IDs with dosage
        YogaAsanas NVARCHAR(MAX) NULL,  -- JSON array of asana IDs with frequency
        DietaryRecommendations NVARCHAR(MAX) NULL,  -- JSON array of dietary items
        LifestyleModifications NVARCHAR(MAX) NULL,
        Duration NVARCHAR(100) NULL,
        ConfidenceScore DECIMAL(5,2) NULL,
        Explanation NVARCHAR(MAX) NULL,
        CreatedBy INT NULL,  -- Doctor/Admin user ID
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        
        CONSTRAINT FK_TreatmentPlans_Patients FOREIGN KEY (PatientId) 
            REFERENCES PATIENTS(Id),
        CONSTRAINT FK_TreatmentPlans_Conditions FOREIGN KEY (ConditionId) 
            REFERENCES CONDITIONS(Id)
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_TreatmentPlans_PatientId ON TREATMENTPLANS(PatientId);
    CREATE NONCLUSTERED INDEX IX_TreatmentPlans_ConditionId ON TREATMENTPLANS(ConditionId);
    CREATE NONCLUSTERED INDEX IX_TreatmentPlans_Prakriti ON TREATMENTPLANS(Prakriti);
    CREATE NONCLUSTERED INDEX IX_TreatmentPlans_CreatedAt ON TREATMENTPLANS(CreatedAt DESC);
    CREATE NONCLUSTERED INDEX IX_TreatmentPlans_IsActive ON TREATMENTPLANS(IsActive) WHERE IsActive = 1;
    
    PRINT '  ✓ TREATMENTPLANS table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ TREATMENTPLANS table already exists';
END
GO

-- =============================================
-- Table 9: TREATMENTOUTCOMES
-- Description: Track treatment effectiveness for ML learning
-- =============================================
PRINT 'Step 9/10: Creating TREATMENTOUTCOMES table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TREATMENTOUTCOMES')
BEGIN
    CREATE TABLE TREATMENTOUTCOMES (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        TreatmentPlanId BIGINT NOT NULL,
        PatientId BIGINT NOT NULL,
        EffectivenessScore INT NULL,  -- 1-10 scale
        SideEffects NVARCHAR(MAX) NULL,
        PatientFeedback NVARCHAR(MAX) NULL,
        DoctorNotes NVARCHAR(MAX) NULL,
        FollowUpDate DATETIME2 NULL,
        RecordedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        RecordedBy INT NULL,  -- Doctor user ID
        
        CONSTRAINT FK_TreatmentOutcomes_TreatmentPlans FOREIGN KEY (TreatmentPlanId) 
            REFERENCES TREATMENTPLANS(Id) ON DELETE CASCADE,
        CONSTRAINT FK_TreatmentOutcomes_Patients FOREIGN KEY (PatientId) 
            REFERENCES PATIENTS(Id)
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_TreatmentOutcomes_TreatmentPlanId ON TREATMENTOUTCOMES(TreatmentPlanId);
    CREATE NONCLUSTERED INDEX IX_TreatmentOutcomes_PatientId ON TREATMENTOUTCOMES(PatientId);
    CREATE NONCLUSTERED INDEX IX_TreatmentOutcomes_EffectivenessScore ON TREATMENTOUTCOMES(EffectivenessScore);
    CREATE NONCLUSTERED INDEX IX_TreatmentOutcomes_RecordedAt ON TREATMENTOUTCOMES(RecordedAt DESC);
    
    PRINT '  ✓ TREATMENTOUTCOMES table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ TREATMENTOUTCOMES table already exists';
END
GO

-- =============================================
-- Table 10: CONDITIONTREATMENTMAPPING
-- Description: Pre-configured treatment mappings for recommendation engine
-- =============================================
PRINT 'Step 10/10: Creating CONDITIONTREATMENTMAPPING table...';

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CONDITIONTREATMENTMAPPING')
BEGIN
    CREATE TABLE CONDITIONTREATMENTMAPPING (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        ConditionId BIGINT NOT NULL,
        TreatmentType NVARCHAR(50) NOT NULL,  -- Medicine/Yoga/Diet
        TreatmentItemId BIGINT NOT NULL,  -- FK to respective table
        Prakriti NVARCHAR(50) NULL,  -- Specific prakriti or 'All'
        Priority INT NOT NULL,  -- 1=Primary, 2=Secondary, 3=Supportive
        SuccessRate DECIMAL(5,2) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        
        CONSTRAINT FK_ConditionTreatmentMapping_Conditions FOREIGN KEY (ConditionId) 
            REFERENCES CONDITIONS(Id) ON DELETE CASCADE
    );
    
    -- Create Indexes
    CREATE NONCLUSTERED INDEX IX_ConditionTreatmentMapping_ConditionId ON CONDITIONTREATMENTMAPPING(ConditionId);
    CREATE NONCLUSTERED INDEX IX_ConditionTreatmentMapping_TreatmentType ON CONDITIONTREATMENTMAPPING(TreatmentType);
    CREATE NONCLUSTERED INDEX IX_ConditionTreatmentMapping_Priority ON CONDITIONTREATMENTMAPPING(Priority);
    CREATE NONCLUSTERED INDEX IX_ConditionTreatmentMapping_Prakriti ON CONDITIONTREATMENTMAPPING(Prakriti);
    CREATE NONCLUSTERED INDEX IX_ConditionTreatmentMapping_IsActive ON CONDITIONTREATMENTMAPPING(IsActive) WHERE IsActive = 1;
    CREATE NONCLUSTERED INDEX IX_ConditionTreatmentMapping_Composite ON CONDITIONTREATMENTMAPPING(ConditionId, TreatmentType, Prakriti, Priority);
    
    PRINT '  ✓ CONDITIONTREATMENTMAPPING table created successfully';
END
ELSE
BEGIN
    PRINT '  ⚠ CONDITIONTREATMENTMAPPING table already exists';
END
GO

-- =============================================
-- Verification Summary
-- =============================================
PRINT '';
PRINT '========================================';
PRINT 'Treatment Recommendation System Tables Created Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Summary of created tables:';
PRINT '  1. PATIENTS - Patient information and Prakriti';
PRINT '  2. PRAKRITIQUESTIONS - Assessment questionnaire';
PRINT '  3. PRAKRITIASSESSMENTS - Patient assessment responses';
PRINT '  4. CONDITIONS - Medical conditions database';
PRINT '  5. HERBALMEDICINES - Herbal medicine database';
PRINT '  6. YOGAASANAS - Yoga postures database';
PRINT '  7. DIETARYITEMS - Food items with properties';
PRINT '  8. TREATMENTPLANS - Generated recommendations';
PRINT '  9. TREATMENTOUTCOMES - Treatment effectiveness tracking';
PRINT ' 10. CONDITIONTREATMENTMAPPING - Treatment mappings';
PRINT '';
PRINT 'Next steps:';
PRINT '  1. Run SeedTreatmentData.sql to populate with sample data';
PRINT '  2. Run CreateTreatmentStoredProcedures.sql to add stored procedures';
PRINT '';
GO

-- =============================================
-- Vedic API Database - Treatment Recommendation System Stored Procedures
-- Description: Stored procedures for treatment recommendation operations
-- Date: 2026-02-15
-- =============================================

USE VedicDB;
GO

PRINT '========================================';
PRINT 'Creating Treatment Recommendation System Stored Procedures';
PRINT '========================================';
PRINT '';

-- =============================================
-- Procedure 1: sp_GetPrakritiQuestions
-- Description: Retrieve all active Prakriti assessment questions
-- =============================================
PRINT 'Step 1/12: Creating sp_GetPrakritiQuestions...';

CREATE OR ALTER PROCEDURE sp_GetPrakritiQuestions
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Category,
        Question,
        VataOption,
        PittaOption,
        KaphaOption,
        DisplayOrder
    FROM PRAKRITIQUESTIONS
    WHERE IsActive = 1
    ORDER BY DisplayOrder;
END
GO
PRINT '  ✓ sp_GetPrakritiQuestions created';

-- =============================================
-- Procedure 2: sp_SavePrakritiAssessment
-- Description: Save patient Prakriti assessment results
-- =============================================
PRINT 'Step 2/12: Creating sp_SavePrakritiAssessment...';

CREATE OR ALTER PROCEDURE sp_SavePrakritiAssessment
    @PatientId BIGINT,
    @Responses NVARCHAR(MAX),
    @VataScore INT,
    @PittaScore INT,
    @KaphaScore INT,
    @DominantPrakriti NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Insert assessment
        INSERT INTO PRAKRITIASSESSMENTS (PatientId, Responses, VataScore, PittaScore, KaphaScore, DominantPrakriti)
        VALUES (@PatientId, @Responses, @VataScore, @PittaScore, @KaphaScore, @DominantPrakriti);
        
        DECLARE @AssessmentId BIGINT = SCOPE_IDENTITY();
        
        -- Update patient's Prakriti
        DECLARE @PrakritiScore NVARCHAR(100) = 
            '{"vata":' + CAST(@VataScore AS NVARCHAR(10)) + 
            ',"pitta":' + CAST(@PittaScore AS NVARCHAR(10)) + 
            ',"kapha":' + CAST(@KaphaScore AS NVARCHAR(10)) + '}';
        
        UPDATE PATIENTS
        SET Prakriti = @DominantPrakriti,
            PrakritiScore = @PrakritiScore,
            UpdatedAt = GETUTCDATE()
        WHERE Id = @PatientId;
        
        COMMIT TRANSACTION;
        
        -- Return the assessment
        SELECT 
            Id,
            PatientId,
            VataScore,
            PittaScore,
            KaphaScore,
            DominantPrakriti,
            AssessedAt
        FROM PRAKRITIASSESSMENTS
        WHERE Id = @AssessmentId;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        THROW;
    END CATCH
END
GO
PRINT '  ✓ sp_SavePrakritiAssessment created';

-- =============================================
-- Procedure 3: sp_GetPatientByUserId
-- Description: Get patient details linked to a user
-- =============================================
PRINT 'Step 3/12: Creating sp_GetPatientByUserId...';

CREATE OR ALTER PROCEDURE sp_GetPatientByUserId
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        UserId,
        Name,
        Age,
        Gender,
        ContactNumber,
        Email,
        Address,
        Prakriti,
        PrakritiScore,
        CreatedAt,
        UpdatedAt
    FROM PATIENTS
    WHERE UserId = @UserId AND IsActive = 1;
END
GO
PRINT '  ✓ sp_GetPatientByUserId created';

-- =============================================
-- Procedure 4: sp_GetPatientById
-- Description: Get patient details by patient ID
-- =============================================
PRINT 'Step 4/12: Creating sp_GetPatientById...';

CREATE OR ALTER PROCEDURE sp_GetPatientById
    @PatientId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        UserId,
        Name,
        Age,
        Gender,
        ContactNumber,
        Email,
        Address,
        Prakriti,
        PrakritiScore,
        CreatedAt,
        UpdatedAt
    FROM PATIENTS
    WHERE Id = @PatientId AND IsActive = 1;
END
GO
PRINT '  ✓ sp_GetPatientById created';

-- =============================================
-- Procedure 5: sp_SearchConditions
-- Description: Search conditions by name, symptoms, or category
-- =============================================
PRINT 'Step 5/12: Creating sp_SearchConditions...';

CREATE OR ALTER PROCEDURE sp_SearchConditions
    @SearchTerm NVARCHAR(200) = NULL,
    @Category NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Name,
        SanskritName,
        Category,
        Description,
        CommonSymptoms,
        AffectedDoshas,
        Severity
    FROM CONDITIONS
    WHERE IsActive = 1
        AND (@Category IS NULL OR Category = @Category)
        AND (@SearchTerm IS NULL OR 
             Name LIKE N'%' + @SearchTerm + N'%' OR
             SanskritName LIKE N'%' + @SearchTerm + N'%' OR
             Description LIKE N'%' + @SearchTerm + N'%' OR
             CommonSymptoms LIKE N'%' + @SearchTerm + N'%')
    ORDER BY Name;
END
GO
PRINT '  ✓ sp_SearchConditions created';

-- =============================================
-- Procedure 6: sp_GetHerbalMedicines
-- Description: Get herbal medicines with optional filters
-- =============================================
PRINT 'Step 6/12: Creating sp_GetHerbalMedicines...';

CREATE OR ALTER PROCEDURE sp_GetHerbalMedicines
    @SearchTerm NVARCHAR(200) = NULL,
    @PrakritiEffect NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        CommonName,
        SanskritName,
        ScientificName,
        HindiName,
        Properties,
        Indications,
        Dosage,
        Contraindications,
        SideEffects,
        ImageUrl,
        VataEffect,
        PittaEffect,
        KaphaEffect
    FROM HERBALMEDICINES
    WHERE IsActive = 1
        AND (@SearchTerm IS NULL OR 
             CommonName LIKE N'%' + @SearchTerm + N'%' OR
             SanskritName LIKE N'%' + @SearchTerm + N'%' OR
             HindiName LIKE N'%' + @SearchTerm + N'%' OR
             Indications LIKE N'%' + @SearchTerm + N'%')
        AND (@PrakritiEffect IS NULL OR
             (@PrakritiEffect = N'Vata' AND VataEffect = N'Balancing') OR
             (@PrakritiEffect = N'Pitta' AND PittaEffect = N'Balancing') OR
             (@PrakritiEffect = N'Kapha' AND KaphaEffect = N'Balancing'))
    ORDER BY CommonName;
END
GO
PRINT '  ✓ sp_GetHerbalMedicines created';

-- =============================================
-- Procedure 7: sp_GetYogaAsanas
-- Description: Get yoga asanas with optional filters
-- =============================================
PRINT 'Step 7/12: Creating sp_GetYogaAsanas...';

CREATE OR ALTER PROCEDURE sp_GetYogaAsanas
    @Category NVARCHAR(100) = NULL,
    @Difficulty NVARCHAR(50) = NULL,
    @PrakritiEffect NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        AsanaName,
        SanskritName,
        Category,
        Benefits,
        Duration,
        Difficulty,
        Instructions,
        Precautions,
        ImageUrl,
        VideoUrl,
        VataEffect,
        PittaEffect,
        KaphaEffect
    FROM YOGAASANAS
    WHERE IsActive = 1
        AND (@Category IS NULL OR Category = @Category)
        AND (@Difficulty IS NULL OR Difficulty = @Difficulty)
        AND (@PrakritiEffect IS NULL OR
             (@PrakritiEffect = N'Vata' AND VataEffect = N'Balancing') OR
             (@PrakritiEffect = N'Pitta' AND PittaEffect = N'Balancing') OR
             (@PrakritiEffect = N'Kapha' AND KaphaEffect = N'Balancing'))
    ORDER BY Category, Difficulty, AsanaName;
END
GO
PRINT '  ✓ sp_GetYogaAsanas created';

-- =============================================
-- Procedure 8: sp_GetTreatmentRecommendations
-- Description: Get treatment recommendations for a condition and prakriti
-- =============================================
PRINT 'Step 8/12: Creating sp_GetTreatmentRecommendations...';

CREATE OR ALTER PROCEDURE sp_GetTreatmentRecommendations
    @ConditionId BIGINT,
    @Prakriti NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get medicines
    SELECT 
        m.Id,
        m.CommonName,
        m.SanskritName,
        m.Dosage,
        m.Indications,
        m.Contraindications,
        ctm.Priority,
        ctm.SuccessRate,
        ctm.Notes
    FROM CONDITIONTREATMENTMAPPING ctm
    INNER JOIN HERBALMEDICINES m ON ctm.TreatmentItemId = m.Id
    WHERE ctm.ConditionId = @ConditionId
        AND ctm.TreatmentType = N'Medicine'
        AND ctm.IsActive = 1
        AND m.IsActive = 1
        AND (ctm.Prakriti = @Prakriti OR ctm.Prakriti = N'All')
    ORDER BY ctm.Priority, ctm.SuccessRate DESC;
    
    -- Get yoga asanas
    SELECT 
        y.Id,
        y.AsanaName,
        y.SanskritName,
        y.Category,
        y.Benefits,
        y.Duration,
        y.Difficulty,
        y.Instructions,
        y.Precautions,
        ctm.Priority,
        ctm.SuccessRate,
        ctm.Notes
    FROM CONDITIONTREATMENTMAPPING ctm
    INNER JOIN YOGAASANAS y ON ctm.TreatmentItemId = y.Id
    WHERE ctm.ConditionId = @ConditionId
        AND ctm.TreatmentType = N'Yoga'
        AND ctm.IsActive = 1
        AND y.IsActive = 1
        AND (ctm.Prakriti = @Prakriti OR ctm.Prakriti = N'All')
    ORDER BY ctm.Priority, ctm.SuccessRate DESC;
    
    -- Get dietary recommendations based on prakriti
    SELECT 
        Id,
        FoodName,
        Category,
        VataEffect,
        PittaEffect,
        KaphaEffect,
        Benefits,
        Rasa,
        Virya
    FROM DIETARYITEMS
    WHERE IsActive = 1
        AND (
            (@Prakriti LIKE N'%Vata%' AND VataEffect = N'Balancing') OR
            (@Prakriti LIKE N'%Pitta%' AND PittaEffect = N'Balancing') OR
            (@Prakriti LIKE N'%Kapha%' AND KaphaEffect = N'Balancing')
        )
    ORDER BY Category, FoodName;
END
GO
PRINT '  ✓ sp_GetTreatmentRecommendations created';

-- =============================================
-- Procedure 9: sp_SaveTreatmentPlan
-- Description: Save a generated treatment plan
-- =============================================
PRINT 'Step 9/12: Creating sp_SaveTreatmentPlan...';

CREATE OR ALTER PROCEDURE sp_SaveTreatmentPlan
    @PatientId BIGINT,
    @ConditionId BIGINT,
    @Prakriti NVARCHAR(50),
    @HerbalMedicines NVARCHAR(MAX),
    @YogaAsanas NVARCHAR(MAX),
    @DietaryRecommendations NVARCHAR(MAX),
    @LifestyleModifications NVARCHAR(MAX),
    @Duration NVARCHAR(100),
    @ConfidenceScore DECIMAL(5,2),
    @Explanation NVARCHAR(MAX),
    @CreatedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        INSERT INTO TREATMENTPLANS (
            PatientId, ConditionId, Prakriti, HerbalMedicines, YogaAsanas,
            DietaryRecommendations, LifestyleModifications, Duration,
            ConfidenceScore, Explanation, CreatedBy
        )
        VALUES (
            @PatientId, @ConditionId, @Prakriti, @HerbalMedicines, @YogaAsanas,
            @DietaryRecommendations, @LifestyleModifications, @Duration,
            @ConfidenceScore, @Explanation, @CreatedBy
        );
        
        DECLARE @TreatmentPlanId BIGINT = SCOPE_IDENTITY();
        
        -- Return the created treatment plan
        SELECT 
            tp.Id,
            tp.PatientId,
            p.Name AS PatientName,
            tp.ConditionId,
            c.Name AS ConditionName,
            tp.Prakriti,
            tp.HerbalMedicines,
            tp.YogaAsanas,
            tp.DietaryRecommendations,
            tp.LifestyleModifications,
            tp.Duration,
            tp.ConfidenceScore,
            tp.Explanation,
            tp.CreatedAt
        FROM TREATMENTPLANS tp
        INNER JOIN PATIENTS p ON tp.PatientId = p.Id
        INNER JOIN CONDITIONS c ON tp.ConditionId = c.Id
        WHERE tp.Id = @TreatmentPlanId;
        
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
PRINT '  ✓ sp_SaveTreatmentPlan created';

-- =============================================
-- Procedure 10: sp_GetTreatmentHistory
-- Description: Get treatment history for a patient
-- =============================================
PRINT 'Step 10/12: Creating sp_GetTreatmentHistory...';

CREATE OR ALTER PROCEDURE sp_GetTreatmentHistory
    @PatientId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        tp.Id,
        tp.PatientId,
        p.Name AS PatientName,
        tp.ConditionId,
        c.Name AS ConditionName,
        c.SanskritName AS ConditionSanskritName,
        tp.Prakriti,
        tp.Duration,
        tp.ConfidenceScore,
        tp.CreatedAt,
        -- Get outcome if exists
        (SELECT TOP 1 EffectivenessScore 
         FROM TREATMENTOUTCOMES 
         WHERE TreatmentPlanId = tp.Id 
         ORDER BY RecordedAt DESC) AS EffectivenessScore
    FROM TREATMENTPLANS tp
    INNER JOIN PATIENTS p ON tp.PatientId = p.Id
    INNER JOIN CONDITIONS c ON tp.ConditionId = c.Id
    WHERE tp.PatientId = @PatientId
        AND tp.IsActive = 1
    ORDER BY tp.CreatedAt DESC;
END
GO
PRINT '  ✓ sp_GetTreatmentHistory created';

-- =============================================
-- Procedure 11: sp_SaveTreatmentOutcome
-- Description: Record treatment outcome/feedback
-- =============================================
PRINT 'Step 11/12: Creating sp_SaveTreatmentOutcome...';

CREATE OR ALTER PROCEDURE sp_SaveTreatmentOutcome
    @TreatmentPlanId BIGINT,
    @PatientId BIGINT,
    @EffectivenessScore INT = NULL,
    @SideEffects NVARCHAR(MAX) = NULL,
    @PatientFeedback NVARCHAR(MAX) = NULL,
    @DoctorNotes NVARCHAR(MAX) = NULL,
    @FollowUpDate DATETIME2 = NULL,
    @RecordedBy INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Validate effectiveness score
        IF @EffectivenessScore IS NOT NULL AND (@EffectivenessScore < 1 OR @EffectivenessScore > 10)
        BEGIN
            RAISERROR('Effectiveness score must be between 1 and 10', 16, 1);
            RETURN;
        END
        
        INSERT INTO TREATMENTOUTCOMES (
            TreatmentPlanId, PatientId, EffectivenessScore, SideEffects,
            PatientFeedback, DoctorNotes, FollowUpDate, RecordedBy
        )
        VALUES (
            @TreatmentPlanId, @PatientId, @EffectivenessScore, @SideEffects,
            @PatientFeedback, @DoctorNotes, @FollowUpDate, @RecordedBy
        );
        
        DECLARE @OutcomeId BIGINT = SCOPE_IDENTITY();
        
        -- Return the outcome
        SELECT 
            Id,
            TreatmentPlanId,
            PatientId,
            EffectivenessScore,
            SideEffects,
            PatientFeedback,
            DoctorNotes,
            FollowUpDate,
            RecordedAt
        FROM TREATMENTOUTCOMES
        WHERE Id = @OutcomeId;
        
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
PRINT '  ✓ sp_SaveTreatmentOutcome created';

-- =============================================
-- Procedure 12: sp_GetTreatmentStatistics
-- Description: Get dashboard statistics
-- =============================================
PRINT 'Step 12/12: Creating sp_GetTreatmentStatistics...';

CREATE OR ALTER PROCEDURE sp_GetTreatmentStatistics
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Overall statistics
    SELECT 
        'TotalPatients' AS Metric,
        COUNT(*) AS Value
    FROM PATIENTS
    WHERE IsActive = 1
    
    UNION ALL
    
    SELECT 
        'TotalTreatmentPlans' AS Metric,
        COUNT(*) AS Value
    FROM TREATMENTPLANS
    WHERE IsActive = 1
    
    UNION ALL
    
    SELECT 
        'TotalConditions' AS Metric,
        COUNT(*) AS Value
    FROM CONDITIONS
    WHERE IsActive = 1
    
    UNION ALL
    
    SELECT 
        'TotalHerbalMedicines' AS Metric,
        COUNT(*) AS Value
    FROM HERBALMEDICINES
    WHERE IsActive = 1
    
    UNION ALL
    
    SELECT 
        'TotalYogaAsanas' AS Metric,
        COUNT(*) AS Value
    FROM YOGAASANAS
    WHERE IsActive = 1
    
    UNION ALL
    
    SELECT 
        'AverageConfidenceScore' AS Metric,
        CAST(AVG(ConfidenceScore) AS DECIMAL(5,2)) AS Value
    FROM TREATMENTPLANS
    WHERE IsActive = 1 AND ConfidenceScore IS NOT NULL
    
    UNION ALL
    
    SELECT 
        'AverageEffectivenessScore' AS Metric,
        CAST(AVG(CAST(EffectivenessScore AS DECIMAL(5,2))) AS DECIMAL(5,2)) AS Value
    FROM TREATMENTOUTCOMES
    WHERE EffectivenessScore IS NOT NULL;
    
    -- Prakriti distribution
    SELECT 
        Prakriti,
        COUNT(*) AS PatientCount
    FROM PATIENTS
    WHERE IsActive = 1 AND Prakriti IS NOT NULL
    GROUP BY Prakriti
    ORDER BY PatientCount DESC;
    
    -- Most common conditions
    SELECT TOP 10
        c.Name AS ConditionName,
        COUNT(tp.Id) AS TreatmentCount
    FROM CONDITIONS c
    LEFT JOIN TREATMENTPLANS tp ON c.Id = tp.ConditionId AND tp.IsActive = 1
    WHERE c.IsActive = 1
    GROUP BY c.Name
    ORDER BY TreatmentCount DESC;
    
    -- Most prescribed medicines
    SELECT TOP 10
        m.CommonName,
        m.SanskritName,
        COUNT(ctm.Id) AS PrescriptionCount
    FROM HERBALMEDICINES m
    LEFT JOIN CONDITIONTREATMENTMAPPING ctm ON m.Id = ctm.TreatmentItemId 
        AND ctm.TreatmentType = N'Medicine' AND ctm.IsActive = 1
    WHERE m.IsActive = 1
    GROUP BY m.CommonName, m.SanskritName
    ORDER BY PrescriptionCount DESC;
END
GO
PRINT '  ✓ sp_GetTreatmentStatistics created';

-- =============================================
-- Additional Helper Procedures
-- =============================================

-- Procedure: sp_GetConditionById
PRINT 'Creating sp_GetConditionById...';
CREATE OR ALTER PROCEDURE sp_GetConditionById
    @ConditionId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Name,
        SanskritName,
        Category,
        Description,
        CommonSymptoms,
        AffectedDoshas,
        Severity
    FROM CONDITIONS
    WHERE Id = @ConditionId AND IsActive = 1;
END
GO
PRINT '  ✓ sp_GetConditionById created';

-- Procedure: sp_GetTreatmentPlanById
PRINT 'Creating sp_GetTreatmentPlanById...';
CREATE OR ALTER PROCEDURE sp_GetTreatmentPlanById
    @TreatmentPlanId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        tp.Id,
        tp.PatientId,
        p.Name AS PatientName,
        p.Age AS PatientAge,
        p.Gender AS PatientGender,
        tp.ConditionId,
        c.Name AS ConditionName,
        c.SanskritName AS ConditionSanskritName,
        c.Description AS ConditionDescription,
        tp.Prakriti,
        tp.HerbalMedicines,
        tp.YogaAsanas,
        tp.DietaryRecommendations,
        tp.LifestyleModifications,
        tp.Duration,
        tp.ConfidenceScore,
        tp.Explanation,
        tp.CreatedAt,
        tp.UpdatedAt
    FROM TREATMENTPLANS tp
    INNER JOIN PATIENTS p ON tp.PatientId = p.Id
    INNER JOIN CONDITIONS c ON tp.ConditionId = c.Id
    WHERE tp.Id = @TreatmentPlanId AND tp.IsActive = 1;
    
    -- Get outcomes for this treatment plan
    SELECT 
        Id,
        EffectivenessScore,
        SideEffects,
        PatientFeedback,
        DoctorNotes,
        FollowUpDate,
        RecordedAt
    FROM TREATMENTOUTCOMES
    WHERE TreatmentPlanId = @TreatmentPlanId
    ORDER BY RecordedAt DESC;
END
GO
PRINT '  ✓ sp_GetTreatmentPlanById created';

-- Procedure: sp_CreatePatient
PRINT 'Creating sp_CreatePatient...';
CREATE OR ALTER PROCEDURE sp_CreatePatient
    @UserId INT = NULL,
    @Name NVARCHAR(200),
    @Age INT,
    @Gender NVARCHAR(10),
    @ContactNumber NVARCHAR(15) = NULL,
    @Email NVARCHAR(200) = NULL,
    @Address NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        INSERT INTO PATIENTS (UserId, Name, Age, Gender, ContactNumber, Email, Address)
        VALUES (@UserId, @Name, @Age, @Gender, @ContactNumber, @Email, @Address);
        
        DECLARE @PatientId BIGINT = SCOPE_IDENTITY();
        
        SELECT 
            Id,
            UserId,
            Name,
            Age,
            Gender,
            ContactNumber,
            Email,
            Address,
            Prakriti,
            CreatedAt
        FROM PATIENTS
        WHERE Id = @PatientId;
        
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
PRINT '  ✓ sp_CreatePatient created';

-- Procedure: sp_GetDietaryItemsByPrakriti
PRINT 'Creating sp_GetDietaryItemsByPrakriti...';
CREATE OR ALTER PROCEDURE sp_GetDietaryItemsByPrakriti
    @Prakriti NVARCHAR(50),
    @Category NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        FoodName,
        Category,
        VataEffect,
        PittaEffect,
        KaphaEffect,
        Properties,
        Benefits,
        Rasa,
        Virya
    FROM DIETARYITEMS
    WHERE IsActive = 1
        AND (@Category IS NULL OR Category = @Category)
        AND (
            (@Prakriti LIKE N'%Vata%' AND VataEffect = N'Balancing') OR
            (@Prakriti LIKE N'%Pitta%' AND PittaEffect = N'Balancing') OR
            (@Prakriti LIKE N'%Kapha%' AND KaphaEffect = N'Balancing')
        )
    ORDER BY Category, FoodName;
END
GO
PRINT '  ✓ sp_GetDietaryItemsByPrakriti created';

-- =============================================
-- Final Summary
-- =============================================
PRINT '';
PRINT '========================================';
PRINT 'Treatment Recommendation Stored Procedures Created Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Created procedures:';
PRINT '  1. sp_GetPrakritiQuestions - Get assessment questions';
PRINT '  2. sp_SavePrakritiAssessment - Save assessment results';
PRINT '  3. sp_GetPatientByUserId - Get patient by user ID';
PRINT '  4. sp_GetPatientById - Get patient by patient ID';
PRINT '  5. sp_SearchConditions - Search conditions';
PRINT '  6. sp_GetHerbalMedicines - Get medicines with filters';
PRINT '  7. sp_GetYogaAsanas - Get yoga asanas';
PRINT '  8. sp_GetTreatmentRecommendations - Get recommendations';
PRINT '  9. sp_SaveTreatmentPlan - Save treatment plan';
PRINT ' 10. sp_GetTreatmentHistory - Get patient history';
PRINT ' 11. sp_SaveTreatmentOutcome - Save outcome/feedback';
PRINT ' 12. sp_GetTreatmentStatistics - Dashboard statistics';
PRINT '';
PRINT 'Helper procedures:';
PRINT '  - sp_GetConditionById';
PRINT '  - sp_GetTreatmentPlanById';
PRINT '  - sp_CreatePatient';
PRINT '  - sp_GetDietaryItemsByPrakriti';
PRINT '';
PRINT 'Database setup complete! Ready for API integration.';
PRINT '';
GO

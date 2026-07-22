-- ============================================================================
-- VEDIC AI PORTAL - DATABASE MIGRATION SCRIPT
-- Script: CreateAiRecommendationsSupport.sql
-- Description: Adds AI generation metadata columns for feature-flagged AI recommendations
-- ============================================================================

USE [vedic_ai];
GO

-- 1. Add AI Recommendation Metadata columns to TreatmentPlans table if not exists
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[TreatmentPlans]') AND name = N'IsAiGenerated')
BEGIN
    ALTER TABLE [dbo].[TreatmentPlans]
    ADD [IsAiGenerated] BIT NOT NULL DEFAULT 0,
        [AiModelUsed] NVARCHAR(100) NULL,
        [ClinicalRationale] NVARCHAR(MAX) NULL,
        [AnupanaInstructions] NVARCHAR(MAX) NULL,
        [PathyaApathyaJson] NVARCHAR(MAX) NULL;
    PRINT 'Added AI metadata columns to [TreatmentPlans] table.';
END
ELSE
BEGIN
    PRINT '[TreatmentPlans] table already contains AI metadata columns.';
END
GO

-- 2. Create AI Generation Audit Log Table for tracking usage and cost/tokens
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AiAuditLogs]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AiAuditLogs] (
        [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
        [FeatureName] NVARCHAR(100) NOT NULL, -- 'Recommendations', 'SushrutaAssistant', etc.
        [PatientId] BIGINT NULL,
        [ConditionId] BIGINT NULL,
        [Provider] NVARCHAR(50) NOT NULL, -- 'Gemini', 'Groq', 'Ollama', 'Fallback'
        [ModelName] NVARCHAR(100) NOT NULL,
        [PromptTokens] INT NULL DEFAULT 0,
        [CompletionTokens] INT NULL DEFAULT 0,
        [IsSuccessful] BIT NOT NULL DEFAULT 1,
        [ErrorMessage] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [FK_AiAuditLogs_Patients] FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Patients]([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_AiAuditLogs_Conditions] FOREIGN KEY ([ConditionId]) REFERENCES [dbo].[Conditions]([Id]) ON DELETE SET NULL
    );
    PRINT 'Created [AiAuditLogs] table for tracking feature-flagged AI usage.';
END
ELSE
BEGIN
    PRINT '[AiAuditLogs] table already exists.';
END
GO

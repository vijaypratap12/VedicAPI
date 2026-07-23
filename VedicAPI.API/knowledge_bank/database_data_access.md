# Database Architecture & Data Access

This document details the backend database structure, direct SQL scripting migrations, and repository data-access layer.

---

## 1. Data Access Stack

The database utilizes **Microsoft SQL Server**. Rather than an ORM like Entity Framework Core, the project uses **Dapper** (a lightweight micro-ORM) for fast, direct SQL execution.

```mermaid
graph LR
    Repository[Repository Class] --> Conn[SqlConnection]
    Conn --> Dapper[Dapper Query/Execute]
    Dapper --> SQL[SQL Server]
```

### Key Repository Pattern details:
*   **Connection Lifecycle:** Connection strings are read via `IConfiguration`. Repositories instantiate a new connection for each query block:
    ```csharp
    using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    ```
*   **Async Operations:** All data fetches are asynchronous, using Dapper methods like `QueryAsync<T>`, `QueryFirstOrDefaultAsync<T>`, and `ExecuteAsync`.

---

## 2. Database Scripts Catalog (`Database/`)

Database creation, schemas, and seeding data are managed using raw SQL script files.

### A. Core System Tables
*   `CreateDatabase.sql`: Initializes the Dev database.
*   `CreateUsersTable.sql`: Generates `Users` table (tracks Email, Password Hash, Practitioner Role, License Number).
*   `CreateUserOTPTable.sql`: Generates `UserOTPs` tracking reset tokens and expiration timers.

### B. Library & Academic Tables
*   `CreateTextbooksTable.sql`: Creates `Textbooks` and `TextbookChapters` indexes.
*   `MigrateToChapters.sql` / `AddSampleContentHtml.sql`: Populates textbook content in HTML formats to support rich web reading interfaces.
*   `CreateResearchAndThesisTable.sql`: Creates `ResearchPapers` and `Theses` tables for comparative studies.

### C. Clinical & Treatment tables
*   `CreateTreatmentRecommendationTables.sql`: Generates tables for:
    *   `Patients` (Bio-stats, clinical notes, Prakriti flags).
    *   `Conditions` (Sanskrit name, symptoms, affected doshas).
    *   `HerbalMedicines` (Sanskrit, scientific names, standard dosages).
    *   `YogaAsanas` (Postures, durations, difficulties).
    *   `DietaryItems` (Prakriti benefits, categories).
    *   `TreatmentPlans` (Active plans, rationales, serialized revisions, outcome status).
    *   `TreatmentOutcomes` (Patient progress visit scores, side effects).
*   `SeedTreatmentData.sql`: Large seed script containing extensive classical mappings of Ayurvedic herbs, conditions, and yoga practices.
*   `CreateTreatmentStoredProcedures.sql`: Contains optimized stored procedures for pulling combined clinical matches.

---

## 3. Stored Procedure & Query Performance

The recommendation engine relies on stored procedures or structured SQL queries to retrieve candidate recommendations. For example, repositories query candidate tables by checking if the item aligns with the patient's Prakriti:

```sql
SELECT * FROM HerbalMedicines 
WHERE Id IN (
    SELECT MedicineId FROM ConditionMedicineMappings 
    WHERE ConditionId = @ConditionId
) AND (PrakritiEffect = @Prakriti OR PrakritiEffect = 'All' OR @Prakriti = 'All')
```
This query ensures that selected candidates are aligned with the patient's Prakriti balance.

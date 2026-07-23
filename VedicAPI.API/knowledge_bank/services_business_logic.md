# Business Logic & Service Layer

This document details the backend service layer, dependency injection (DI) lifecycles, and core business calculation logic.

---

## 1. Dependency Injection Lifetimes

Service and repository classes are registered in `Program.cs` under three distinct lifecycles:

### A. Scoped (`AddScoped`)
Instantiated once per HTTP request context. This is the standard for repositories and business services:
*   **Services:** `IBookService`, `ITextbookService`, `IAuthService`, `IPatientService`, `IPrakritiAssessmentService`, `ITreatmentRecommendationService`, `ITreatmentPlanService`.
*   **Repositories:** `IBookRepository`, `IAuthRepository`, `IPatientRepository`, `ITreatmentRepository`, etc.

### B. Transient (`AddTransient`)
Instantiated every time they are requested from the container. Used for resolving multiple client drivers of the same interface:
*   `IVedicAiClient` mappings (`GeminiAiClient` and `GroqAiClient`).

### C. Singleton (`AddSingleton`)
Instantiated once and shared across the entire application lifetime:
*   `VedicAiClientFactory` (does not hold request state).

---

## 2. Core Service Implementations

### A. Patient Prakriti Scoring (`PrakritiAssessmentService.cs`)
*   **Responsibility:** Evaluates Prakriti questions and calculates the biological constitution.
*   **Logic:**
    *   Tallies the responses associated with physical and behavioral traits (Vata, Pitta, Kapha).
    *   Translates quantitative totals into constitutional classifications (e.g. "Vata-Pitta" or "Tridosha").
    *   Stores answers and the final score on the `PrakritiAssessment` database record.

### B. Treatment recommendations (`TreatmentRecommendationService.cs`)
*   **Responsibility:** The core gateway resolving rule-based fallbacks or AI recommenders.
*   **AI Path:** Resolves `IVedicAiClient` via factory, injects database grounding candidate items, and maps response models.
*   **Rule Engine Fallback:** If AI is disabled or fails, maps condition mapping matrices using repositories, computes a hardcoded confidence score (usually 60-90% depending on Prakriti assessment completeness), and formats static lifestyle cards.

### C. Treatment Plans & Revisions (`TreatmentPlanService.cs`)
*   **Responsibility:** Stores plan DTOs and audits adjustments.
*   **Versioning Audit Trail:** When updating plans, the service accepts a clinical explanation. It appends a history stamp object (containing DoctorName, Timestamp, Explanation, and deep copies of Plan elements) to a serialized JSON array (`RevisionHistory`), saving it to the database to ensure a clear audit trail.

---

## 3. Registration Setup (`Program.cs`)

```csharp
// Repositories Scoped
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPrakritiRepository, PrakritiRepository>();

// Services Scoped
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPrakritiAssessmentService, PrakritiAssessmentService>();

// AI Clients & Factories
builder.Services.AddHttpClient<GeminiAiClient>();
builder.Services.AddHttpClient<GroqAiClient>();
builder.Services.AddTransient<IVedicAiClient, GeminiAiClient>();
builder.Services.AddTransient<IVedicAiClient, GroqAiClient>();
builder.Services.AddSingleton<VedicAiClientFactory>();
```

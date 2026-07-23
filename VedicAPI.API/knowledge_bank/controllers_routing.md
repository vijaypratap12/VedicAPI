# API Endpoints, Routing, & Controller Reference

This document inventories all API endpoints exposed by the C# Web API project.

---

## 1. Routing Conventions & Response Mapping

*   **Route Prefix:** All endpoints are prefixed under `api/[controller]` or direct route attributes (e.g. `api/treatment-recommendations`).
*   **Standardized Envelope:** Every endpoint returns JSON payload formatted inside a generic `ApiResponse<T>` model:
    ```json
    {
      "success": true,
      "message": "Action completed successfully",
      "data": { ... },
      "errors": []
    }
    ```
*   **Case Conventions:** Properties are defined in C# using PascalCase (e.g., `PatientName`) but are serialized to JSON as camelCase (e.g., `patientName`).

---

## 2. API Controllers Catalog

### A. Authentication & Recovery (`AuthController.cs`)
*   `POST api/auth/register` – Registers a new practitioner.
*   `POST api/auth/login` – Authenticates credentials, returning a JWT token.
*   `GET api/auth/current-user` – Retrieves practitioner details using token headers.
*   `POST api/auth/forgot-password` – Initiates a verification code request (OTP).
*   `POST api/auth/verify-otp` – Validates recovery OTP tokens.
*   `POST api/auth/reset-password` – Finalizes credentials resets.

### B. Patient Management (`PatientsController.cs`)
*   `GET api/patients` – Lists all practitioner-assigned patient files.
*   `POST api/patients` – Registers a new patient record.
*   `GET api/patients/{id}` – Fetches patient profile.
*   `PUT api/patients/{id}` – Modifies biographical details.
*   `DELETE api/patients/{id}` – Removes patient history logs.

### C. Prakriti Scoring (`PrakritiController.cs`)
*   `POST api/prakriti/assessments/{patientId}` – Saves a patient's Prakriti questionnaire answers and constitution type.
*   `GET api/prakriti/assessments/{patientId}` – Retrieves past assessment histories.

### D. Treatment Recommendations (`TreatmentRecommendationsController.cs`)
*   `GET api/treatment-recommendations/ai-status` – Gets features configuration statuses.
*   `POST api/treatment-recommendations/generate` – Generates a therapeutic plan based on condition and Prakriti (supports optional AI generation).
*   `POST api/treatment-recommendations/suggest-adjustment` – Evaluates outcomes logs and returns AI-generated plan adjustments.

### E. Treatment Plans (`TreatmentPlansController.cs`)
*   `GET api/treatment-plans` – Lists active therapeutic plans.
*   `POST api/treatment-plans` – Saves recommendations as active plans.
*   `GET api/treatment-plans/{id}` – Retrieves plan detail, revision history, and outcomes.
*   `PUT api/treatment-plans/{id}` – Updates plan contents, creating audit trails.
*   `DELETE api/treatment-plans/{id}` – Archive/deletes plans.
*   `POST api/treatment-plans/{planId}/outcomes` – Records patient visit outcomes.

### F. Textbooks & Libraries (`TextbooksController.cs` & `BooksController.cs`)
*   `GET api/textbooks` – Directory of digital textbooks.
*   `GET api/textbooks/{bookId}/chapters` – Directory of chapters for a textbook.
*   `GET api/textbooks/chapters/{chapterId}` – Reading content/HTML for specific textbook chapters.
*   `GET api/books` / `GET api/books/{bookId}/chapters` – Reads original classical texts (like Sushruta Samhita).

### G. Catalogs & Resources
*   `GET api/catalog/medicines` – Lists AYUSH herbal catalog formulations.
*   `GET api/catalog/yoga` – Lists yoga poses and durations.
*   `GET api/catalog/dietary` – Lists food recommendations scoped by Prakriti.
*   `GET api/research-papers` / `GET api/thesis` – Lists historical thesis papers and research entries.
*   `GET api/jurisprudence` – Retrieves legal and ethical codes.

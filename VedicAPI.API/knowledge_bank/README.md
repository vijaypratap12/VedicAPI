# Vedic API - Backend Knowledge Bank

Welcome to the backend knowledge bank for the `VedicAPI` project. The backend is designed as an ASP.NET Core Web API using .NET 10. It handles data access through Dapper (lightweight ORM) mapping to a SQL Server database, and integrates external AI clients (Gemini and Groq) to serve clinical diagnostics and adjustments.

## ⚙️ Backend Tech Stack
*   **Framework:** .NET 10 (ASP.NET Core Web API)
*   **Database Access:** Dapper & Microsoft.Data.SqlClient (raw SQL/stored procedure-driven repositories)
*   **Database:** SQL Server (schema managed via SQL script updates under `Database/`)
*   **Security:** JWT-based bearer authentication and SHA256/BCrypt password hashing
*   **AI Service Integration:** Native HTTP clients connecting to Google Gemini (`gemini-1.5-flash`) and Groq Cloud

---

## 📂 Knowledge Bank Directory Map

To explore a specific aspect of the backend project, choose one of the dedicated documentation files below:

| Documentation File | Description |
| :--- | :--- |
| **[ai_architecture.md](./ai_architecture.md)** | Service factory patterns (`VedicAiClientFactory`), Gemini/Groq HTTP integrations, JSON schema enforcement prompts, and feature flags. |
| **[controllers_routing.md](./controllers_routing.md)** | API endpoint mappings (Auth, Patients, recommendations, Textbooks, Jurisprudence), route naming conventions, and DTO layouts. |
| **[services_business_logic.md](./services_business_logic.md)** | Core business operations, dependency injection setups, Prakriti calculations, and audit revision histories. |
| **[database_data_access.md](./database_data_access.md)** | Table structures, stored procedures, sample seeds, and repository class Dapper implementations. |
| **[auth_security.md](./auth_security.md)** | User identity tables, JWT signature credentials, OTP request tokens, and security filters. |

---

## 🚀 Execution & Local Running

### 1. Build Verification
Make sure you have the .NET 10 SDK installed, and compile the solution:
```bash
dotnet build
```

### 2. Configuration (`appsettings.json`)
Configure your SQL Server connection strings and AI secrets inside `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;User Id=...;Password=...;"
  },
  "AiSettings": {
    "Enabled": true,
    "Provider": "Gemini",
    "ApiKey": "YOUR_GEMINI_API_KEY",
    "ModelName": "gemini-1.5-flash"
  }
}
```

### 3. Execution
Launch the Web API development profile (runs on Swagger UI by default at `https://localhost:7136/swagger`):
```bash
dotnet run --project VedicAPI.API
```

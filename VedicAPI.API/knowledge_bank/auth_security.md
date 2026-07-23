# Identity, Authentication & Security

This document outlines the authentication policies, password security rules, and user recovery systems implemented on the Web API.

---

## 1. Password Security & Cryptography

*   **Hashing:** The backend enforces secure credential storage. Passwords are encrypted before being written to the SQL Server database.
*   **Verification:** During login, user credentials are verified by comparing the incoming password hash with the one stored in the database.

---

## 2. JWT Bearer Token Authentication

The backend uses **JWT (JSON Web Token)** bearer authentication to secure API endpoints.

### A. JWT Configuration Profile (`appsettings.json`)
```json
"JwtSettings": {
  "SecretKey": "VedicAI-Super-Secret-Key-For-JWT-Token-Generation-2025-Min-32-Chars",
  "Issuer": "VedicAPI",
  "Audience": "VedicAI",
  "ExpiryInHours": 24,
  "RefreshTokenExpiryInDays": 7
}
```

### B. Validation Rules (`Program.cs`)
Tokens submitted in HTTP headers (`Authorization: Bearer <token>`) are validated against these parameters:
*   **Signature Match:** Ensures the token was signed with the backend's secret key.
*   **Issuer and Audience:** Matches valid configurations (`VedicAPI` and `VedicAI`).
*   **Lifetime Checks:** Blocks expired tokens.
*   **Clock Skew:** Set to `TimeSpan.Zero` to enforce exact expiration times without delay.

---

## 3. Protecting API Routes

Endpoints are protected by registering the authentication middleware in the HTTP request pipeline:
```csharp
app.UseAuthentication();
app.UseAuthorization();
```
*   **Access Control:** Controllers or individual actions requiring authentication are decorated with the `[Authorize]` attribute.
*   **Role-Based Access:** Standard practitioner routes check for general authorization. Admin or specialized clinical routes can enforce specific roles if needed.

---

## 4. OTP Password Recovery Flow

For password recovery, the backend uses a database-backed **OTP (One-Time Password)** token system:

1.  **Request OTP (`api/auth/forgot-password`):**
    *   Generates a random numeric code (length configured by `OTPSettings:OTPLength`, default `6`).
    *   Saves the OTP code to the `UserOTPs` database table along with an expiration time (`OTPSettings:OTPExpiryMinutes`, default `10` minutes).
2.  **Verify OTP (`api/auth/verify-otp`):**
    *   Validates the code and ensures it hasn't expired.
    *   Returns a success flag, allowing the user to reset their password.
3.  **Reset Password (`api/auth/reset-password`):**
    *   Accepts the new password, hashes it, and updates the user's password record.
    *   Deletes or invalidates the used OTP token from the database.

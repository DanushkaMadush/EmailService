# EmailService (Email Microservice API)

**EmailService** is a lightweight **ASP.NET Core (.NET 8)** API for sending emails via **SMTP**.  
It’s designed to be added as a **microservice** in company systems (HR systems, helpdesks, ERP/CRM apps, notification platforms, etc.) to send automated emails from a central service.

## Features

- Send emails via **SMTP** (MailKit)
- Supports:
  - Multiple **To** recipients
  - Optional **CC** recipients
  - **HTML body** + appended **signature**
  - **File attachments** (upload via `multipart/form-data`)
- Swagger UI enabled in Development
- SQL Server + EF Core registered (for persistence / logging / stored-procedure based operations)

---

## Tech Stack

- **.NET 8** (ASP.NET Core Web API)
- **MailKit** (SMTP client)
- **Entity Framework Core** + **SQL Server**
- **Swagger / OpenAPI** (Swashbuckle)

---

## Project Structure (high-level)

- `API/Controllers/`  
  - `EmailController.cs` (email sending endpoint)
- `Infrastructure/Services/`  
  - `SmtpMailService.cs` (SMTP implementation using MailKit)
- `Infrastructure/Settings/`  
  - `SmtpSettings.cs` (SMTP config binding)
- `Infrastructure/Database/`  
  - EF Core DbContext (registered in `Program.cs`)
- `Application/`, `Domain/`, `Shared/`  
  - Layered architecture (DTOs, interfaces, common utilities)

---

## Running the API

### Prerequisites
- .NET SDK 8.x
- Access to an SMTP server (company SMTP, Gmail SMTP, Office365 SMTP, SendGrid SMTP relay, etc.)
- SQL Server connection string (required by startup)

### Configure settings

`Program.cs` expects:

- `ConnectionStrings:DefaultConnection`
- `SmtpSettings:Host`
- `SmtpSettings:Port`
- `SmtpSettings:User`
- `SmtpSettings:Password`

Create an `appsettings.json` in the repository root (or provide these via environment variables in production):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EmailServiceDb;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True;"
  },
  "SmtpSettings": {
    "Host": "smtp.your-company.com",
    "Port": 587,
    "User": "no-reply@your-company.com",
    "Password": "your-smtp-password"
  }
}
```

> The repo currently includes `appsettings.Development.json` only (logging). Add the above values to `appsettings.json` or your secret store.

### Start the service

From the repo root:

```bash
dotnet restore
dotnet run
```

Default development URLs (from `Properties/launchSettings.json`):
- `http://localhost:5134`
- `https://localhost:7074`

Swagger UI (Development):
- `http://localhost:5134/swagger`

---

## API Endpoints

### Send Email (with optional attachments)

**POST** `/api/email/send`

- Consumes: `multipart/form-data`
- Request binding in controller:
  - `[FromForm] EmailRequestDto emailRequest`
  - `List<IFormFile>? files`

#### EmailRequestDto schema

```csharp
public class EmailRequestDto
{
    public string From { get; set; }
    public List<string> To { get; set; }
    public List<string>? Cc { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Signature { get; set; }
}
```

#### Attachments

- Optional form field: `files` (can be repeated)
- Each file is added as an email attachment

---

## Example Requests

### 1) Send email (no attachments)

```bash
curl -X POST "http://localhost:5134/api/email/send" \
  -H "Content-Type: multipart/form-data" \
  -F "From=no-reply@your-company.com" \
  -F "To=recipient1@company.com" \
  -F "To=recipient2@company.com" \
  -F "Cc=manager@company.com" \
  -F "Subject=Test Email" \
  -F "Body=<h3>Hello</h3><p>This is a test email.</p>" \
  -F "Signature=HelpDeskPro Notification Service"
```

### 2) Send email with attachments

```bash
curl -X POST "http://localhost:5134/api/email/send" \
  -H "Content-Type: multipart/form-data" \
  -F "From=no-reply@your-company.com" \
  -F "To=recipient@company.com" \
  -F "Subject=Report" \
  -F "Body=Please find the attached report." \
  -F "Signature=EmailService" \
  -F "files=@./document.pdf" \
  -F "files=@./image.png"
```

---

## Deployment as a Microservice

Common approaches:
- Run as a standalone service behind an API Gateway (recommended)
- Containerize with Docker (ideal for microservices)
- Store secrets in:
  - Environment variables
  - AWS SSM / Secrets Manager
  - Azure Key Vault
  - HashiCorp Vault

**Important:** Do not commit SMTP passwords or DB connection strings to source control.

---

## Troubleshooting

- If startup fails, verify `ConnectionStrings:DefaultConnection` exists.
- If email sending fails:
  - verify SMTP host/port
  - verify credentials
  - ensure the SMTP server supports STARTTLS on the configured port (commonly 587)
  - check firewall/network rules

---

## License

See [`LICENSE`](./LICENSE).
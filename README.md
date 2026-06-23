# Deployment Journey Documentation Site
# Deployment Journey Documentation Site

A documentation website for a deployment and DevOps learning portfolio. The solution contains a Blazor WebAssembly frontend, an ASP.NET Core Web API backend, shared domain models, and SQL Server persistence.

The application is containerized with Docker and deployed on a virtual machine. The VM can be accessed with SSH, while deployment automation is handled through GitHub Actions and Dokploy.

## Projects

| Project | Description |
| --- | --- |
| `Blazor` | Blazor WebAssembly frontend served locally by `dotnet run` or in Docker by nginx. |
| `API` | ASP.NET Core Web API with documentation and health endpoints. |
| `DomainModels` | Shared DTOs and models used by both the frontend and backend. |

## Main features

- Documentation milestone cards based on the current krav sections.
- Table of Contents links that navigate to each milestone card.
- Editable milestone content through the UI using an edit key.
- Static skills checklist showing learned and not-yet-learned course topics.
- Health status badges for API and database connectivity.
- SQL Server persistence for saved milestone content.
- EF Core migrations run automatically on API startup.
- Docker-based deployment on a VM.
- Automated deployment flow with GitHub Actions and Dokploy.

## Documentation sections

The website currently shows these milestone sections:

1. Project Description
2. Infrastructure & Deployment
3. CI/CD Pipeline
4. Security
5. Monitoring & Operations
6. Learning & Reflection

Rows are not seeded into the database. The API derives the placeholder cards from `DocumentationCategoryEnum`, and a database row is created only when a section is saved from the UI.

## Prerequisites

- .NET 8 SDK
- SQL Server Express for local development, or Docker Desktop for Docker-based development
- PowerShell

## Local development without Docker

### 1. Build the solution

```powershell
cd C:\Users\vwi\source\repos\DoDLJ
dotnet build
```

### 2. Run the API

Open a PowerShell terminal:

```powershell
cd C:\Users\vwi\source\repos\DoDLJ\API
dotnet run
```

The API runs on:

```text
http://localhost:5012
```

Swagger is available at:

```text
http://localhost:5012/swagger
```

### 3. Run the Blazor frontend

Open a second PowerShell terminal:

```powershell
cd C:\Users\vwi\source\repos\DoDLJ\Blazor
dotnet run
```

The frontend runs on:

```text
http://localhost:5123
```

In development, `Blazor/wwwroot/appsettings.Development.json` points the frontend to the local API:

```json
{
  "ApiBaseUrl": "http://localhost:5012/"
}
```

## Local database

The default local connection string is in `API/appsettings.json`:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=DeploymentJourneyDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

The API runs pending EF Core migrations automatically on startup using `Database.Migrate()`.

To apply migrations manually:

```powershell
cd C:\Users\vwi\source\repos\DoDLJ\API
dotnet ef database update
```

## Edit key

Editing documentation from the UI requires `DocumentationEditApiKey`.

Enter the key in the frontend's edit-key field and enable `Edit mode`.

## Docker and deployment

The application can run as a Docker-based setup. The `docker-compose.yml` file contains three services:

- `sqlserver`
- `api`
- `blazor`

For local Docker testing, start the full stack from the repository root:

```powershell
cd C:\Users\vwi\source\repos\DoDLJ
docker compose up --build
```

Open the frontend at:

```text
http://localhost:8080
```

Stop containers:

```powershell
docker compose down
```

In the deployed setup, Docker runs on a VM. The VM can be accessed with SSH for maintenance, logs, and troubleshooting. The deployment process is automated with GitHub Actions and Dokploy, so changes can be pushed from GitHub and deployed without manually copying files to the server.

## Frontend API configuration

The Blazor app supports environment-specific API configuration in a simple way:

- Local development calls the API directly on `http://localhost:5012/`.
- Docker/production uses same-origin `/api` requests through nginx.

In production, nginx serves the Blazor frontend and forwards `/api` requests to the backend API container.

## Notes

- The skills checklist is static UI data and is not stored in the database.
- Milestone cards are always shown from the enum list, even before anything is saved.


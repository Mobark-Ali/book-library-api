# Book Library Management API

Full-stack book library app: **ASP.NET Core 8 Web API** + **EF Core** + **SQL Server**, with JWT auth, FluentValidation, tests, a front-end, and Docker.

## Stack
- ASP.NET Core 8 Web API
- EF Core (Code First + Migrations)
- SQL Server 2022
- JWT Authentication (BCrypt password hashing)
- FluentValidation
- xUnit + EF Core InMemory + FluentAssertions
- Vanilla JS front-end
- Docker + docker-compose

## Features
- Full CRUD (Books, Authors)
- Register/Login with JWT
- Role-based authorization (Admin can delete)
- Request validation via FluentValidation
- Swagger with Bearer auth support
- Auto migrations + seeded admin on startup
- Unit tests for controller logic
- Responsive front-end consuming the API
- One-command Docker deployment

## Run locally
```bash
dotnet restore
dotnet ef database update
dotnet run

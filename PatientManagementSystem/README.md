# Patient Management System - .NET 7 Web API (Dapper)

This repository contains a ready-to-open Visual Studio solution with:

- PatientManagement.API (ASP.NET Core Web API using Dapper)
- PatientManagement.Tests (xUnit + Moq tests)
- SQL initialization script at `/sql/init.sql`

What you need to do after downloading:
1. Replace the connection string in `src/PatientManagement.API/appsettings.json` with your SQL Server connection string.
2. Run the SQL script `/sql/init.sql` to create the database and tables.
3. Open `PatientManagement.sln` in Visual Studio 2022/2023 or use `dotnet` CLI.
4. Restore NuGet packages and build.
5. Run the API project (it uses minimal hosting). Swagger is enabled in Development.
6. For authentication samples, the JWT key is in appsettings.json — replace it with a secure secret.

Notes & Next steps:
- Add more endpoints for Conditions and PatientConditions (pattern already established).
- Harden JWT token creation and add identity management or an authentication server.
- Add integration tests and CI/CD pipeline.

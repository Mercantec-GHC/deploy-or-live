\# Deployment Journey



Deployment Journey is a small full-stack learning project built as part of a DevOps course Deploy or Die.



The goal is not to build a complex application, but to learn how a real application is structured, connected to a database, tested locally, and later deployed.



\## Tech Stack



\- .NET 8

\- Blazor WebAssembly

\- ASP.NET Core Web API

\- Entity Framework Core

\- SQL Server Express

\- Visual Studio 2022



\## Solution Structure



```text

DoDLJ

├── API

│   ├── Controllers

│   ├── Data

│   ├── Migrations

│   └── Program.cs

│

├── Blazor

│   ├── Interfaces

│   ├── Layout

│   ├── Models

│   ├── Pages

│   ├── Services

│   └── wwwroot/appsettings.json

│

└── DomainModels

&#x20;   ├── Dto

&#x20;   └── Models


# Nimbleloop AI Business Analyst

An AI-powered Business Analyst agent that converts use cases, business flows, and client requirements into properly written features, design guidelines, and user stories using LLM API calls.

## Architecture

### Overview

This application follows a **vertical slice architecture** built on a **.NET Blazor WebApp**. The server project hosts both the Blazor front-end (rendered via WebAssembly) and the backend (via minimal APIs), keeping the deployment simple and the codebase easy to navigate.

```
Nimbleloop.AiBusinessAnaylst.WebApp/
├── Nimbleloop.AiBusinessAnaylst.WebApp/          # Server (ASP.NET Core host)
│   ├── Components/                                # Blazor server-rendered components
│   │   ├── Layout/                                # Shared layout components
│   │   ├── Pages/                                 # Server-side pages
│   │   └── Account/                               # Identity / account management UI
│   ├── Data/                                      # EF Core DbContext & models
│   └── Program.cs                                 # Application entry point & service registration
│
└── Nimbleloop.AiBusinessAnaylst.WebApp.Client/    # Client (Blazor WebAssembly)
    └── Pages/                                     # Interactive client-side pages
```

### Design Principles

- **Vertical Slicing** – Features are organized by capability rather than technical layer. Each feature contains its own models, validation, and endpoints.
- **Minimal Layers** – No unnecessary abstractions. The server project hosts both the UI components and the API surface.
- **Blazor WebApp** – WebAssembly rendering for rich client-side interactivity; the same ASP.NET Core server provides the backend via minimal APIs.

### Key Packages

| Package | Purpose |
|---------|---------|
| **FluentValidation** | Strongly-typed validation rules for request models |
| **MongoDB.EntityFrameworkCore** | EF Core provider for Azure Cosmos DB (MongoDB API) |
| **OpenAI** | Official .NET client for the OpenAI API |
| **Anthropic.SDK** | .NET client for the Anthropic (Claude) API |
| **ASP.NET Core Identity** | Authentication and user management |
| **HttpClient (ClickUp)** | Named HTTP client for the ClickUp API integration |

## Prerequisites

Before running the application, ensure you have the following installed:

### Required

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- An **Azure Cosmos DB (MongoDB API)** instance, **or** a local [MongoDB](https://www.mongodb.com/try/download/community) server (v6.0+)

### Optional (for full functionality)

- An **OpenAI API key** – required for OpenAI-based analysis features
- An **Anthropic API key** – required for Claude-based analysis features
- A **ClickUp API key** – required for ClickUp integration (task syncing)

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/NimbleLoop-Technologies/AiBusinessAnaylst.git
cd AiBusinessAnaylst
```

### 2. Configure the application

Update `appsettings.json` (or use [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) for sensitive values) in the server project:

```json
{
  "ConnectionStrings": {
    "MongoDb": "mongodb://localhost:27017"
  },
  "MongoDb": {
    "DatabaseName": "AiBusinessAnalyst"
  },
  "OpenAI": {
    "ApiKey": "<your-openai-api-key>"
  },
  "Anthropic": {
    "ApiKey": "<your-anthropic-api-key>"
  },
  "ClickUp": {
    "ApiKey": "<your-clickup-api-key>",
    "BaseUrl": "https://api.clickup.com/api/v2"
  }
}
```

> **Tip:** For local development, use `dotnet user-secrets` to store API keys instead of placing them in `appsettings.json`:
> ```bash
> cd Nimbleloop.AiBusinessAnaylst.WebApp/Nimbleloop.AiBusinessAnaylst.WebApp
> dotnet user-secrets set "OpenAI:ApiKey" "<your-key>"
> dotnet user-secrets set "Anthropic:ApiKey" "<your-key>"
> dotnet user-secrets set "ClickUp:ApiKey" "<your-key>"
> ```

### 3. Run the application

```bash
dotnet run --project Nimbleloop.AiBusinessAnaylst.WebApp/Nimbleloop.AiBusinessAnaylst.WebApp
```

The application will start and be available at the URLs specified in `Properties/launchSettings.json` (by default `https://localhost:7130`).

### 4. Build the solution

```bash
dotnet build Nimbleloop.AiBusinessAnaylst.slnx
```

## Configuration

### Azure Cosmos DB (MongoDB API)

To connect to Azure Cosmos DB instead of a local MongoDB instance, use the Cosmos DB connection string:

```
mongodb://<account-name>:<primary-key>@<account-name>.mongo.cosmos.azure.com:10255/?ssl=true&retrywrites=false&replicaSet=globaldb&maxIdleTimeMS=120000&appName=@<account-name>@
```

Set this as the `ConnectionStrings:MongoDb` value in your configuration.

## License

This project is licensed under the Apache License 2.0 – see the [LICENSE.txt](LICENSE.txt) file for details.
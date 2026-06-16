# FantasyEkstraklasaCoach

AI assistant for building and managing your squad on [fantasy.ekstraklasa.org](https://fantasy.ekstraklasa.org).

Instead of spending hours analyzing stats, the app scores every player automatically and uses Claude AI to pick your optimal 15-man squad — then suggests transfers, captain, and formation before each round.

## Stack

| Layer | Technology |
|-------|-----------|
| Backend | .NET 10, FastEndpoints |
| Frontend | Blazor Server |
| Database | PostgreSQL (Supabase) |
| AI | Scoring algorithm + Claude API |
| Data | Scraper — ekstraklasa.org |
| Hosting | Azure App Service for Containers |
| CI/CD | GitHub Actions → Azure Container Registry |

## How it works

```
ekstraklasa.org
  ↓ scraper (cron)
  Players + stats + schedule → PostgreSQL
        ↓
  Scoring algorithm (.NET)
  form × fixture difficulty × price/value × status
        ↓
  Claude API
  picks optimal 15 / suggests transfers, captain, formation
        ↓
  User reviews and confirms → Blazor UI
```

## Squad rules (fantasy.ekstraklasa.org)

| Rule | Value |
|------|-------|
| Budget | 30M |
| Squad size | 15 (11 starters + 4 subs) |
| Goalkeepers | 2 |
| Defenders | 5 |
| Midfielders | 5 |
| Forwards | 3 |
| Max per club | 3 |

## Development

### Prerequisites

- .NET 10 SDK
- Docker + Docker Compose
- Supabase project (or local PostgreSQL)

### Run locally

```bash
dotnet run --project FantasyEkstraklasaCoach.Api   # http://localhost:5210
dotnet run --project FantasyEkstraklasaCoach.Web   # http://localhost:5211
```

Or with Docker Compose (coming soon):

```bash
docker-compose up
```

### Build & test

```bash
dotnet build FantasyEkstraklasaCoach.slnx
dotnet test FantasyEkstraklasaCoach.slnx
```

### Database migrations

```bash
dotnet ef migrations add <MigrationName> \
  --project FantasyEkstraklasaCoach.Infrastructure \
  --startup-project FantasyEkstraklasaCoach.Api

dotnet ef database update \
  --project FantasyEkstraklasaCoach.Infrastructure \
  --startup-project FantasyEkstraklasaCoach.Api
```

## Project structure

```
FantasyEkstraklasaCoach.Domain          # Entities, value objects, domain events
FantasyEkstraklasaCoach.Application     # Use cases, port interfaces
FantasyEkstraklasaCoach.Infrastructure  # EF Core, PostgreSQL, scraper
FantasyEkstraklasaCoach.Contracts       # Shared request/response records
FantasyEkstraklasaCoach.Api             # FastEndpoints, thin HTTP layer
FantasyEkstraklasaCoach.Web             # Blazor Server frontend
```

## Docs

- [`_docs/MVP.md`](_docs/MVP.md) — scope and success criteria
- [`_docs/PRD.md`](_docs/PRD.md) — full product requirements
- [`_docs/backlog.md`](_docs/backlog.md) — feature backlog

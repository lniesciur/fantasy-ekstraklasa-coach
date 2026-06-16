# FantasyEkstraklasaCoach

Clean Architecture + DDD template — .NET 10, FastEndpoints, PostgreSQL (Supabase), Blazor Server.

## New project checklist

- [ ] `FantasyEkstraklasaCoach` → project name (solution, namespaces, project files, folder names) — automated when using `dotnet new my-clean -n YourProjectName`
- [x] Ports in `FantasyEkstraklasaCoach.Api/Properties/launchSettings.json` — API: `5210` / `7210`
- [x] Ports in `FantasyEkstraklasaCoach.Web/Properties/launchSettings.json` — Web: `5211` / `7211`
- [x] Update ports in `.claude/skills/run-apps/SKILL.md` and `.claude/skills/run-apps-mobile/SKILL.md` to match
- [ ] Fill in `appsettings.json` connection strings and Supabase config

## Commands

```bash
dotnet build FantasyEkstraklasaCoach.slnx
dotnet run --project FantasyEkstraklasaCoach.Api
dotnet run --project FantasyEkstraklasaCoach.Web
dotnet test FantasyEkstraklasaCoach.slnx
dotnet test --filter "FullyQualifiedName~TestMethodName"

dotnet ef migrations add <MigrationName> --project FantasyEkstraklasaCoach.Infrastructure --startup-project FantasyEkstraklasaCoach.Api
dotnet ef database update --project FantasyEkstraklasaCoach.Infrastructure --startup-project FantasyEkstraklasaCoach.Api
dotnet ef migrations remove --project FantasyEkstraklasaCoach.Infrastructure --startup-project FantasyEkstraklasaCoach.Api
```

## Architecture

Clean Architecture + DDD. Dependencies always point inward.

```
Domain ← Application ← Infrastructure
              ↑                ↑
          Contracts           Api ← Contracts
                              Web ← Contracts
```

| Project | Role |
|---------|------|
| `FantasyEkstraklasaCoach.Domain` | Entities, value objects, domain events, domain services — no external dependencies |
| `FantasyEkstraklasaCoach.Application` | Use cases only; port interfaces in `Abstractions/`; returns Contracts Response types directly — no separate Output layer |
| `FantasyEkstraklasaCoach.Infrastructure` | EF Core + Npgsql, Supabase Storage; owns `AppDbContext` |
| `FantasyEkstraklasaCoach.Contracts` | Request / Response / Dto records shared between Application, Api, and Web |
| `FantasyEkstraklasaCoach.Api` | FastEndpoints, thin layer; no business logic, no mapping |
| `FantasyEkstraklasaCoach.Web` | Blazor Server frontend; calls Api via HTTP |

## Implementing a New Feature

Use `FantasyEkstraklasaCoach.Api/Endpoints/Example/` and `FantasyEkstraklasaCoach.Application/Example/UseCases/CreateExample/` as the reference implementation.

**1. Domain** (`FantasyEkstraklasaCoach.Domain/<Feature>/`)
- `<Entity>.cs` — inherit `AggregateRoot<TId>` (or `Entity<TId>` for non-roots), factory `Create(...)`, no public setters
- `<EntityId>.cs` — `public record <EntityId>(Guid Value) { public static <EntityId> New() => new(Guid.NewGuid()); }`
- `<Entity>Created.cs` etc. — implement `IDomainEvent`; raise via `RaiseDomainEvent(...)` inside aggregate methods; automatically dispatched by `UnitOfWork.SaveChangesAsync` via `IDomainEventDispatcher`
- `<Feature>Service.cs` — domain service for logic that spans multiple aggregates
- `I<Entity>Repository.cs` — only methods the use case needs

**2. Application** (`FantasyEkstraklasaCoach.Application/<Feature>/UseCases/<UseCase>/`)
- `<UseCase>UseCase.cs` — returns `Result<XxxResponse>` from Contracts, inject `IUnitOfWork`, call `SaveChangesAsync` last; no separate Output record
- Use `Result<T>` from `FantasyEkstraklasaCoach.Domain.Primitives` for domain errors; let infrastructure exceptions propagate
- Factory methods: `Result<T>.Ok(value)` / `Result<T>.Fail("error message")`, non-generic `Result.Ok()` / `Result.Fail(...)` for void operations

**3. Infrastructure** (`FantasyEkstraklasaCoach.Infrastructure/Persistence/`)
- `Repositories/<Entity>Repository.cs` — implements domain interface, uses `AppDbContext`, never calls `SaveChanges`
- `Configurations/<Entity>Configuration.cs` — EF Fluent API, value conversions for strongly-typed ids
- After adding entity: run migration (see Commands)

**4. Contracts** (`FantasyEkstraklasaCoach.Contracts/<Feature>/`)
- `<UseCase>Request.cs` / `<UseCase>Response.cs` — top-level endpoint contracts
- `<Entity>Dto.cs` — nested shape reused across responses (e.g. items in a list)

**5. Api** (`FantasyEkstraklasaCoach.Api/Endpoints/<Feature>/`)
- `<UseCase>Endpoint.cs` — inherit `Endpoint<TRequest, TResponse>`, delegate to use case
- `<UseCase>RequestValidator.cs` — FluentValidation co-located with endpoint
- Domain errors → `409 Conflict` via `AddError` + `Send.ErrorsAsync`
- Success → `201 Created` via `Send.CreatedAtAsync`

**6. Web** (`FantasyEkstraklasaCoach.Web/Components/Pages/<Feature>/`)
- Blazor components consuming Api via `HttpClient`
- Use `*Request` / `*Response` / `*Dto` types from `FantasyEkstraklasaCoach.Contracts`

## DI Registration (Scrutor — auto by naming convention)

- `*UseCase` → scoped, registered as self
- `*Repository` → scoped, registered as implemented interfaces
- `*Service` → scoped, registered as implemented interfaces

Register manually only for special cases (e.g. `IUnitOfWork`, `IDomainEventDispatcher`).

## Domain Events

`AggregateRoot<TId>` implements `IHasDomainEvents`. `UnitOfWork.SaveChangesAsync` pops events from all tracked aggregates and dispatches them via `IDomainEventDispatcher` after saving.

The template ships with `NullDomainEventDispatcher` (no-op). Replace it with a real implementation (e.g. MediatR `IPublisher`) by swapping the registration in `Infrastructure/DependencyInjection.cs`.

## Naming conventions

- Contracts: `*Request` / `*Response` for endpoint contracts, `*Dto` for nested shapes in lists

## Testing

Test projects: `*.Domain.UnitTests`, `*.Application.UnitTests`, `*.IntegrationTests`.

- Use **NSubstitute** for mocking; `Domain.UnitTests` references `FantasyEkstraklasaCoach.Domain`, `Application.UnitTests` references `FantasyEkstraklasaCoach.Application`
- `*.IntegrationTests` uses `ApiFactory` (Testcontainers PostgreSQL + fake Supabase)
- Test naming: `MethodName_WhenCondition_ExpectedOutcome`
- Failure path: `Assert.False(result.IsSuccess)` + `DidNotReceive`
- Success path: `Assert.True(result.IsSuccess)` + verify output + `Received(1)`

## Specs

Feature specifications are in `_specs/active/`. Read the relevant spec before implementing a feature.

## Known Gotchas

- Migrations require **port 5432** (direct connection) — port 6543 (transaction pooler) breaks DDL
- `ConnectionStrings__DefaultConnection` env var must be set before `dotnet ef database update` in CI
- Never commit credentials — use `dotnet user-secrets` locally

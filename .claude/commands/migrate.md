# Command: migrate

Apply pending EF Core migrations to the database.

## Usage

```
/migrate
```

## How it works

EF Core needs `--startup-project FantasyEkstraklasaCoach.Api` to access the DI configuration and resolve
the connection string. The connection string is read from:

1. `dotnet user-secrets` on the Api project (local development)
2. `ConnectionStrings__DefaultConnection` environment variable (CI / production)

## Steps

1. Run the migration:

```bash
dotnet ef database update --project FantasyEkstraklasaCoach.Infrastructure --startup-project FantasyEkstraklasaCoach.Api
```

2. Confirm output ends with `Done.` — if not, report the error to the user.

3. Remind the user: migrations require a **direct connection on port 5432**.
   Port 6543 (Supabase transaction pooler) breaks DDL statements.

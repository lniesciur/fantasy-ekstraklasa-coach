---
name: run-apps
description: >
  Start or restart the API and Web apps.
  Triggers on: "uruchom apki", "przebuduj apki", "zrestartuj apki", "restart apps",
  "run apps", "start apps", "odśwież apki", "uruchom aplikacje", "zrestartuj aplikacje".
  Also invoked by the /run-apps slash command.
---

# Run Apps

Kill any existing processes on ports 5200 and 5201, then start both apps fresh.

## Steps

1. Kill existing processes:
```bash
kill $(lsof -ti:5201) 2>/dev/null; kill $(lsof -ti:5200) 2>/dev/null; sleep 1 && echo "killed"
```

2. Start both apps in background and wait for them to be ready:
```bash
dotnet run --project FantasyEkstraklasaCoach.Api > /tmp/api.log 2>&1 &
dotnet run --project FantasyEkstraklasaCoach.Web > /tmp/web.log 2>&1 &
sleep 6 && grep "Now listening" /tmp/api.log /tmp/web.log
```

3. Confirm to the user:
- **Web:** http://localhost:5201
- **API:** http://localhost:5200

If either process fails to start, show the relevant log file contents to diagnose the error.

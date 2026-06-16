---
name: run-apps-mobile
description: >
  Start apps bound to 0.0.0.0 so they are accessible from iPhone/mobile on local WiFi.
  Triggers on: "uruchom na telefonie", "odpal na mobile", "run mobile", "test na telefonie",
  "uruchom mobile", "dostęp z telefonu".
  Also invoked by the /run-apps-mobile slash command.
---

# Run Apps (Mobile)

Kill existing processes, then start both apps bound to `0.0.0.0` so they're reachable from any device on the local network.

## Steps

1. Kill existing processes:
```bash
kill $(lsof -ti:5200) 2>/dev/null; kill $(lsof -ti:5201) 2>/dev/null; sleep 1 && echo "killed"
```

2. Get local IP:
```bash
ipconfig getifaddr en0
```

3. Start both apps on `0.0.0.0`:
```bash
dotnet run --project FantasyEkstraklasaCoach.Api --urls "http://0.0.0.0:5200" > /tmp/api.log 2>&1 &
dotnet run --project FantasyEkstraklasaCoach.Web --urls "http://0.0.0.0:5201" > /tmp/web.log 2>&1 &
sleep 6 && grep "Now listening" /tmp/api.log /tmp/web.log
```

4. Confirm to the user with the actual IP from step 2 (e.g. `192.168.1.42`):
- **Web (Mac):** http://localhost:5201
- **Web (iPhone):** http://<LOCAL_IP>:5201
- **API (Mac):** http://localhost:5200

If either process fails to start, show the relevant log file contents to diagnose the error.

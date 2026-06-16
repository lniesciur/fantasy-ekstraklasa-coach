# Backlog — FantasyEkstraklasaCoach

## MVP (v1)

### Dane i scraping
- [ ] Scraper zawodników z ekstraklasa.org — imię, klub, pozycja, cena fantasy
- [ ] Scraper statystyk zawodników — gole, asysty, minuty, żółte kartki, clean sheety
- [ ] Scraper harmonogramu kolejek — terminy meczów, drużyny
- [ ] Status zawodnika — dostępny / kontuzjowany / zawieszony (scraping lub ręczna aktualizacja)
- [ ] Cron uruchamiający scraper przed każdą kolejką
- [ ] Import awaryjny danych z pliku CSV / JSON (panel admina)

### Zawodnicy i scoring
- [ ] Baza zawodników — widok listy z filtrowaniem po pozycji, klubie, cenie
- [ ] Algorytm scoringowy — ocena zawodnika: forma (ostatnie n kolejek) × trudność rywala × cena/wartość × status

### Skład użytkownika
- [ ] Generowanie składu przez AI — tryb inicjalny: AI wybiera optymalnych 15 zawodników w ramach budżetu i zasad platformy
- [ ] Widok składu — układ boiska, 11 startowych + 4 rezerwowych
- [ ] Ręczna korekta składu przez użytkownika
- [ ] Walidacja zasad składu — budżet 30 mln, 2 BRA / 5 OBR / 5 POL / 3 NAP, max 3 z jednego klubu

### Sugestie kolejkowe
- [ ] Sugestie AI przed kolejką — transfery (kto wchodzi / kto wychodzi), kapitan, wicekapitan, formacja startowa
- [ ] Uzasadnienie każdej sugestii w języku polskim (Claude API)

### Frontend i design
- [ ] Kolorystyka i styl wizualny zgodny z ekstraklasa.org — zaimplementować jako CSS custom properties w Blazorze:

  | Token | HEX (przybliżony) | Użycie |
  |-------|-------------------|--------|
  | `--color-bg` | `#0a0e1a` | tło strony (bardzo ciemny granat) |
  | `--color-surface` | `#0d1b3e` | nawigacja, karty, panele |
  | `--color-panel` | `#2563eb` | prawy panel, aktywne elementy |
  | `--color-accent` | `#38bdf8` | podkreślenia, aktywna zakładka, tagi (cyan) |
  | `--color-text-primary` | `#ffffff` | tekst główny |
  | `--color-text-secondary` | `#94a3b8` | tekst drugorzędny, etykiety |
  | `--color-score-bg` | `#1e3a8a` | tło wyników meczu, nagłówki dat |

  - Ogólny feel: dark mode, duże kontrasty, akcenty w błękitno-cyjanowej gamie
  - Brak czerwieni — strona używa wyłącznie błękitno-granatowej palety
  - Font: sans-serif (zweryfikować nazwę w devtools → F12 → Computed → font-family)

### Autoryzacja
- [ ] Logowanie przez Supabase Auth
- [ ] Ręczne zakładanie kont przez admina (brak publicznej rejestracji w beta)

### Infrastruktura
- [ ] Dockerfile dla Api i Web
- [ ] docker-compose dla lokalnego środowiska (Api + Web + PostgreSQL)
- [ ] Azure Container Registry — konfiguracja
- [ ] Azure App Service for Containers — deploy Api i Web
- [ ] GitHub Actions — CI/CD: build → push do ACR → deploy na Azure

---

## v2

- [ ] Publiczna rejestracja użytkowników
- [ ] Ligi — tworzenie, zapraszanie znajomych, ranking
- [ ] System transferów z budżetem sezonowym (kupno / sprzedaż między kolejkami)
- [ ] Historia składów i wyników z poprzednich kolejek
- [ ] Powiadomienia e-mail przed deadline'em kolejki
- [ ] Porównanie składu użytkownika z „optymalnym" AI po zamknięciu kolejki
- [ ] Widok mobilny / PWA

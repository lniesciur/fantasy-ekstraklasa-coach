# PRD — FantasyEkstraklasaCoach

Asystent AI pomagający wybierać optymalny skład na platformie [fantasy.ekstraklasa.org](https://fantasy.ekstraklasa.org).

---

## Cel produktu

Skrócić czas potrzebny do podjęcia dobrej decyzji składowej z godzin do minut — przez automatyczną analizę statystyk i rekomendacje AI dopasowane do bieżącej kolejki Ekstraklasy.

---

## Użytkownicy

### Docelowo
Publiczna aplikacja otwarta dla każdego gracza fantasy.ekstraklasa.org.

### MVP (faza beta)
Dostęp ograniczony do 1–2 zaproszonych użytkowników (właściciel + 1 znajomy). Brak publicznej rejestracji — konta zakładane ręcznie.

### Persona główna
**Aktywny gracz fantasy** — zna zasady platformy ekstraklasa.org, śledzi ligę na poziomie kibica, ale nie jest analitykiem. Chce wybrać dobry skład szybko, bez wchodzenia w głęboką statystykę.

---

## Stack

| Warstwa | Technologia |
|---------|-------------|
| Backend / API | .NET 10, FastEndpoints |
| Frontend | Blazor Server |
| Baza danych | PostgreSQL (Supabase) |
| Storage | Supabase Storage |
| AI | Hybryda: algorytm rankingowy (scoring) + Claude API (decyzje i uzasadnienie) |
| Scraping | ekstraklasa.org — zawodnicy, statystyki, harmonogram |
| Hosting | Azure App Service for Containers |
| Konteneryzacja | Docker (lokalnie: docker-compose; produkcja: Azure Container Registry) |
| CI | GitHub Actions |

---

## Features

### v1 (MVP) — termin: ~mid-July 2026

- **Baza zawodników** — lista zgodna z fantasy.ekstraklasa.org (imię, klub, pozycja, cena, statystyki sezonu)
- **Status zawodnika** — dostępny / kontuzjowany / zawieszony
- **Generowanie składu przez AI** — na żądanie użytkownika AI proponuje optymalny skład 15 zawodników (11 + 4 rezerwowych) w ramach budżetu i zasad platformy; użytkownik zatwierdza lub odrzuca
- **Sugestie przed każdą kolejką** — AI analizuje aktualny skład użytkownika i proponuje: transfery (kogo sprzedać / kupić), kapitana, wicekapitana, formację startową; każda sugestia ma krótkie uzasadnienie
- **Scoring zawodników** — algorytm oblicza ocenę każdego zawodnika na podstawie miar: forma (ostatnie n kolejek), trudność rywala, cena/potencjał, status zdrowotny; Claude używa tych ocen do budowania i optymalizacji składu
- **Widok składu** — podgląd aktualnego składu w układzie boiska, z możliwością ręcznej korekty przed zatwierdzeniem
- **Kalendarz kolejek** — harmonogram meczów z oznaczeniem trudności rywala (wpływa na scoring)
- **Scraper danych** — pobieranie zawodników, statystyk i harmonogramu z ekstraklasa.org; uruchamiany jako cron przed każdą kolejką lub ręcznie przez admina
- **Import awaryjny** — wgranie pliku CSV / JSON przez panel admina jako fallback gdy scraper zawiedzie
- **Autoryzacja** — logowanie przez Supabase Auth; konta zakładane ręcznie w beta

### v2

- Publiczna rejestracja
- Ligi i rywalizacja między użytkownikami
- System transferów z budżetem sezonowym
- Historia składów i wyników z poprzednich kolejek
- Powiadomienia (e-mail / push) przed deadline'em kolejki
- Porównanie własnego składu z „optymalnym" po zamknięciu kolejki

---

## Wymagania niefunkcjonalne

- Czas od wejścia na stronę do wyświetlenia rekomendacji AI < 60 s
- Dane zawodników aktualne przed startem każdej kolejki
- Aplikacja działa poprawnie na desktopie (mobile — poza scopem MVP)

---

## Poza scopem MVP

- Publiczna rejestracja i onboarding
- Ligi, rankingi, rywalizacja
- Powiadomienia
- Aplikacja mobilna / PWA
- Scraping wyników w czasie rzeczywistym (scraper uruchamiany cyklicznie, nie live)
- Historia sezonów

---

## Kryteria sukcesu

1. Użytkownik buduje skład 11 zawodników zgodnie z zasadami fantasy.ekstraklasa.org
2. AI zwraca rekomendację z uzasadnieniem w < 60 s
3. Dane są aktualne przed każdą kolejką (import lub cron działa)
4. Użytkownik testowy ocenia rekomendację AI jako „lepszą lub porównywalną z własną" w ≥ 70% przypadków

---

## Zasady budowania składu (fantasy.ekstraklasa.org)

| Reguła | Wartość |
|--------|---------|
| Budżet | 30 mln |
| Liczba zawodników | 15 (11 startowych + 4 rezerwowych) |
| Bramkarze | 2 |
| Obrońcy | 5 |
| Pomocnicy | 5 |
| Napastnicy | 3 |
| Max zawodników z jednego klubu | 3 |

Walidacja tych reguł jest zaimplementowana po stronie aplikacji (przed wysłaniem składu do platformy i przed wygenerowaniem składu przez AI).

---

## Architektura rekomendacji AI

```
ekstraklasa.org
  ↓ scraper (cron / ręcznie)
  Zawodnicy + statystyki + harmonogram → PostgreSQL
        ↓
  Algorytm scoringowy (.NET)
  - ocena każdego zawodnika: forma × trudność rywala × cena/wartość × status
  - output: lista zawodników z oceną liczbową
        ↓
  Claude API (claude-sonnet-4-6)
  - input: oceny zawodników + aktualny skład + zasady platformy
           (budżet, limity klubowe, limit transferów w kolejce)
  - tryb 1 — pierwszy skład: wybiera optymalnych 15 zawodników
  - tryb 2 — kolejka: sugeruje transfery, kapitana, wicekapitana, formację
  - output: propozycja zmian + uzasadnienie po polsku
        ↓
  Użytkownik zatwierdza lub koryguje → Widok składu w Blazor
```

---

## Otwarte pytania

| # | Pytanie | Właściciel | Priorytet |
|---|---------|------------|-----------|
| 1 | ~~Podejście AI~~ — **rozwiązane**: hybryda (algorytm + Claude API) | — | — |
| 2 | ~~Źródło danych~~ — **rozwiązane**: scraping ekstraklasa.org (brak zewnętrznego API) | — | — |
| 3 | ~~Hosting~~ — **rozwiązane**: Docker + Azure App Service for Containers | — | — |
| 4 | ~~Zasady składu~~ — **rozwiązane**: punkty liczy platforma fantasy.ekstraklasa.org; aplikacja implementuje tylko reguły budowania składu (budżet, liczba zawodników per pozycja, limit z jednego klubu) — ręcznie skonfigurowane | — | — |

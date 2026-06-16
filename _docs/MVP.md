# FantasyEkstraklasaCoach (MVP)

Asystent AI do wybierania składu na platformie [fantasy.ekstraklasa.org](https://fantasy.ekstraklasa.org).

---

## Główny problem

Gracze fantasy.ekstraklasa.org spędzają godziny na analizowaniu statystyk zawodników, sprawdzaniu formy, kontuzji i kalendarza meczów — i nadal nie są pewni swoich wyborów. Brakuje narzędzia, które robi tę analizę za nich i mówi wprost: *„tych 11 wystaw w tej kolejce i zmień tych dwóch"*.

---

## Najmniejszy zestaw funkcjonalności (MVP)

### 1. Baza zawodników
- Lista zawodników Ekstraklasy zgodna z danymi fantasy.ekstraklasa.org: imię, klub, pozycja, cena (punkty / koszt fantasy)
- Statystyki za bieżący sezon: gole, asysty, minuty, żółte kartki, clean sheety, forma (ostatnie 5 kolejek)
- Status: dostępny / kontuzjowany / zawieszony

### 2. Budowanie składu
- Interfejs do ułożenia 11 zawodników w wybranej formacji
- Walidacja zasad platformy: limit budżetu, max. zawodników z jednego klubu
- Zapis i podgląd aktualnego składu użytkownika

### 3. Rekomendacje AI
- AI analizuje wystawiony skład i zwraca konkretne sugestie:
  - Których zawodników warto wymienić i na kogo (z krótkim uzasadnieniem)
  - Rekomendowaną formację na daną kolejkę
  - Propozycję kapitana (zawodnik z najwyższym potencjałem punktowym)
- Analiza uwzględnia: formę, trudność najbliższego rywala, status zdrowotny, stosunek ceny do potencjału

### 4. Kalendarz i kolejki
- Widok meczów Ekstraklasy z podziałem na kolejki
- Oznaczenie podwójnych kolejek i trudności rywala (łatwy / średni / trudny)

### 5. Zasilanie danymi
- Import danych z pliku (CSV / JSON) przez panel admina
- Cron do cyklicznego pobierania danych z zewnętrznego API (np. API-Football) — uruchamiany ręcznie lub automatycznie

---

## Co NIE wchodzi w zakres MVP

- Ligi i rywalizacja między użytkownikami
- System transferów z budżetem sezonowym (kupno/sprzedaż)
- Powiadomienia push / e-mail
- Aplikacja mobilna
- Historia poprzednich sezonów
- Komentarze i funkcje społecznościowe
- Scraping wyników w czasie rzeczywistym

---

## Kryteria sukcesu

MVP uznaje się za udane, jeśli:

1. Użytkownik może zbudować skład 11 zawodników zgodnie z zasadami fantasy.ekstraklasa.org
2. AI zwraca konkretną rekomendację zmian z uzasadnieniem opartym na statystykach
3. Dane zawodników są aktualne na bieżącą kolejkę (import działa)
4. Czas od wejścia na stronę do otrzymania rekomendacji AI < 60 sekund

### Soft KPI

Użytkownik testowy uznaje rekomendację AI za „lepszą lub porównywalną z własną" w ≥ 70% przypadków.

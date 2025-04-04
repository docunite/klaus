# Neo Event Sourcing Demo

Dieses Projekt demonstriert eine vollständige Event Sourcing Architektur in .NET 8 mit:

- Aggregates & Snapshots
- EventStore & SnapshotStore auf MongoDB
- Realtime Projection über MongoDB Change Streams
- Worker-Service für Hintergrundverarbeitung
- Papertrail Logging via Serilog
- Docker-basiertes Setup (API, Worker, MongoDB)

---

## 📦 Struktur

- `EventSourcingTest/`: API/Console-Anwendung mit Aggregates, Commands, Snapshots, EventStore
- `EventSourcingWorker/`: .NET Worker-Service für Realtime-Projection via Change Streams
- `docker-compose.yml`: Startet MongoDB, API & Worker
- `NeoEventSourcingDemo.sln`: Lösung zum Öffnen in Visual Studio / Rider

---

## ▶️ Schnellstart

```bash
docker-compose up --build
```

- API wird gestartet (führt `Create` + `Rename` + Projection aus)
- Worker lauscht auf Events & aktualisiert Projection live
- MongoDB unter `localhost:27017` erreichbar

---

## 🔍 Logs in Papertrail

**Wichtig:** In `Program.cs` beider Projekte müssen deine echten Zugangsdaten hinterlegt sein:

```csharp
.WriteTo.Syslog("logsX.papertrailapp.com", 12345, ...)
```

---

## 🧪 Manuell testen (z. B. dotnet CLI)

```bash
cd EventSourcingTest
dotnet run
```

Die Logs erscheinen in der Konsole **und** in Papertrail (falls konfiguriert).

---

## 📊 Daten anzeigen

Nutze MongoDB Compass oder Studio 3T:

- `neo_demo.events`
- `neo_demo.customer_snapshots`
- `neo_demo.customer_readmodels`

---

## ✅ Hinweise

- `.sln` ist enthalten zum direkten Öffnen
- Code ist mit `ValueObject<T>` & `Identity<T>` abstrahiert
- Konverter für JSON-Serialization bereits eingebaut

---

## 🛠️ Erweitern?

- WebAPI oder gRPC Layer
- GraphQL ReadModel
- Cosmos DB statt MongoDB
- Prometheus + Grafana Metrics

Einfach melden – ich helfe gern weiter.
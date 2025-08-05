```mermaid
sequenceDiagram
    participant Host as MarketDataIngestorBackgroundService
    participant Feature as IngestLiveMarketDataFeature

    Host->>Feature: RunAsync(token)
```

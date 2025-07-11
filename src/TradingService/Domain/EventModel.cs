namespace TradingService.Domain;

// It is the metadata that we store about the event in the event store.
public record EventModel<TAggregateId>(
    TAggregateId AggregateId,
    int SequenceNumber,
    DateTime RaisedAt,
    object Payload);

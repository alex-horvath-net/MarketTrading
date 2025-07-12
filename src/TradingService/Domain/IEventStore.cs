namespace TradingService.Domain;

// It is an EventDescription repository.
public interface IEventStore<TAggregateId> {
    IEnumerable<EventDescription<TAggregateId>> GetEvents(TAggregateId aggregateId);
    void AppendEvent(EventDescription<TAggregateId> eventDescription);
    void SaveChanges();
}

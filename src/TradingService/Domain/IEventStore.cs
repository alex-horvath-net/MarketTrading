namespace TradingService.Domain;

// It is an EventModel repository.
public interface IEventStore<TAggregateId> {
    IEnumerable<EventDescription<TAggregateId>> GetEvents(TAggregateId aggregateId);
    void AppendEvent(EventDescription<TAggregateId> eventModel);
    void SaveChanges();
}

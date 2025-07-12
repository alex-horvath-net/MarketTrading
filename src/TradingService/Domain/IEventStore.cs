namespace TradingService.Domain;

// It is an EventModel repository.
public interface IEventStore<TAggregateId> {
    IEnumerable<EventModel<TAggregateId>> GetEvents(TAggregateId aggregateId);
    void AppendEvent(EventModel<TAggregateId> eventModel);
    void SaveChanges();
}

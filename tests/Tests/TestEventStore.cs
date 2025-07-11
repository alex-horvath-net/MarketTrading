using FluentAssertions;
using TradingService.Domain;

namespace Tests;
public class TestEventStore : IEventStore<Guid> {
    public List<EventModel<Guid>> previousEvents = [];

    public List<EventModel<Guid>> newEvents = [];

    public IEnumerable<EventModel<Guid>> GetEvents(Guid aggregateId) {
        return previousEvents
            .Where(e => e.AggregateId == aggregateId)
            .ToList();
    }

    public void AppendEvent(EventModel<Guid> @event) {
        newEvents.Add(@event);
    }

    public void SaveChanges() {
        throw new NotImplementedException();
    }
}

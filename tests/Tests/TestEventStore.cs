using FluentAssertions;
using TradingService.Domain;

namespace Tests;
public class TestEventStore : IEventStore<Guid> {
    public List<EventDescription<Guid>> previousEvents = [];

    public List<EventDescription<Guid>> newEvents = [];

    public IEnumerable<EventDescription<Guid>> GetEvents(Guid aggregateId) {
        return previousEvents
            .Where(e => e.AggregateId == aggregateId)
            .ToList();
    }

    public void AppendEvent(EventDescription<Guid> @event) {
        newEvents.Add(@event);
    }

    public void SaveChanges() {
        throw new NotImplementedException();
    }
}

using FluentAssertions;
using TradingService.Domain;

namespace Tests;

public abstract class TestCommandHandler<TCommand> {
    protected readonly Guid _defaultAggregateId = Guid.NewGuid();

    protected abstract CommandHandler<TCommand, Guid> Handler { get; }

    protected TestEventStore fakeEventStore = new();

    protected void Given(params object[] previousEvents) {
        Given(_defaultAggregateId, previousEvents);
    }

    protected void Given(Guid aggregateId, params object[] previousEvents) {
        fakeEventStore.previousEvents.AddRange(previousEvents
            .Select((e, i) => new EventModel<Guid>(aggregateId, i, DateTime.Now, e)));
    }

    protected void When(TCommand command) {
        Handler.Handle(command);
    }

    protected void Then(params object[] expectedEvents) {
        Then(_defaultAggregateId, expectedEvents);
    }

    protected void Then(Guid aggregateId, params object[] expectedEvents) {
        var actualEvents = fakeEventStore.newEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.SequenceNumber)
            .Select(e => e.Payload)
            .ToArray();

        actualEvents.Length.Should().Be(expectedEvents.Length);

        for (var i = 0; i < actualEvents.Length; i++) {
            actualEvents[i].Should().BeOfType(expectedEvents[i].GetType());
            try {
                actualEvents[i].Should().BeEquivalentTo(expectedEvents[i]);
            } catch (InvalidOperationException e) {
                // Empty event with matching type is OK. This means that the event class
                // has no properties. If the types match in this situation, the correct
                // event has been appended. So we should ignore this exception.
                if (!e.Message.StartsWith("No members were found for comparison."))
                    throw;
            }
        }
    }
}
using FluentAssertions;
using TradingService.Domain;

namespace Tests;
public abstract class Test_Language<TCommand> {
    protected readonly Guid _defaultAggregateId = Guid.NewGuid();

    protected abstract CommandHandler<TCommand, Guid> CommandHandler { get; }

    protected TestEventStore testEventStore = new();

    protected ITime testTime = new TestTime();
     
    protected void Given(params object[] previousEvents) {
        Given(_defaultAggregateId, previousEvents);
    }

    protected void Given(Guid aggregateId, params object[] previousEvents) {
        testEventStore.previousEvents.AddRange(previousEvents
            .Select((e, i) => new EventDescription<Guid>(aggregateId, i, DateTime.Now, e)));
    }

    protected void When(TCommand command) {
        CommandHandler.Handle(command);
    }

    protected void Then(params object[] expectedEvents) {
        Then(_defaultAggregateId, expectedEvents);
    }

    protected void Then(Guid aggregateId, params object[] expectedEvents) {
        var actualEvents = testEventStore.newEvents
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.SequenceNumber)
            .Select(e => e.BusinessEvent)
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
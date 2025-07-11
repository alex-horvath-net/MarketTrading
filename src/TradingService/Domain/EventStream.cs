namespace TradingService.Domain;

// It represents the event store adapter of the AgregetRoot.
public class EventStream<TAggregateRoot, TAggregateId>(
    IEventStore<TAggregateId> eventStore,
    TAggregateId aggregateId)
    where TAggregateRoot : AggregateRoot<TAggregateId>, new() {
    private int _lastSequenceNumber;

    public TAggregateRoot GetAggregateRoot() {
        var eventModels = eventStore.GetEvents(aggregateId);

        var aggregateRoot = new TAggregateRoot();
        foreach (var eventModel in eventModels) {
            aggregateRoot.Apply(eventModel.Payload);
            _lastSequenceNumber = eventModel.SequenceNumber;
        }

        return aggregateRoot;
    }

    public void Append(object businessEvent) {
        _lastSequenceNumber++;

        var eventModel = new EventModel<TAggregateId>(
            aggregateId,
            _lastSequenceNumber,
            DateTime.UtcNow,
            businessEvent
        );

        eventStore.AppendEvent(eventModel);
    }
}

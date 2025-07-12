namespace TradingService.Domain;

// It represents the event store adapter of the AgregetRoot.
public class EventStream<TAggregateRoot, TAggregateId>(
    IEventStore<TAggregateId> eventStore,
    TAggregateId aggregateId)    where TAggregateRoot : AggregateRoot<TAggregateId>, new() {
    
    public TAggregateRoot GetAggregateRoot() {
        var aggregateRoot = new TAggregateRoot();

        var eventModels = eventStore.GetEvents(aggregateId);
        foreach (var eventModel in eventModels) {
            aggregateRoot.Apply(eventModel.BusinessEvent);
            _lastSequenceNumber = eventModel.SequenceNumber;
        }

        return aggregateRoot;
    }

    public void Append(object businessEvent) {
        _lastSequenceNumber++;

        var eventModel = new EventDescription<TAggregateId>(
            aggregateId,
            _lastSequenceNumber,
            DateTime.UtcNow,
            businessEvent
        );

        eventStore.AppendEvent(eventModel);
    }

    private int _lastSequenceNumber;
}

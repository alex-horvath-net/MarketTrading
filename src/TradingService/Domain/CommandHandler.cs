namespace TradingService.Domain;

public abstract class CommandHandler<TCommand, TAggregateId>(
    IEventStore<TAggregateId> eventStore) {

    protected EventStream<TAggregateRoot, TAggregateId>
      GetEventStream<TAggregateRoot>(TAggregateId aggregateId)
        where TAggregateRoot : AggregateRoot<TAggregateId>, new() =>
        new EventStream<TAggregateRoot, TAggregateId>(
            eventStore,
            aggregateId);

    public abstract void Handle(TCommand command);
}

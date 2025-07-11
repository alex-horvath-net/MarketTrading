namespace TradingService.Domain;

public abstract class AggregateRoot<TAggregateId> {
    public TAggregateId Id { get; protected set; }
    public void Apply(object businessEvent) =>
        ((dynamic)this).Apply((dynamic)businessEvent);
}

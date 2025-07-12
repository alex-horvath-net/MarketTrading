namespace TradingService.Domain.Orders.ValueTypes;
// It is an immutable type defined only by its value, not by identity.
// It represents a descriptive aspect of the domain.
public record Price(
    decimal Value) {

    // I prefer to use explicit validation, so the business logic can split the flow or react in another way.
    public bool IsValid() {
        if (Value <= 0)
           return false;

        return true;
    }

    public override string ToString() => Value.ToString("F2");
}

using Business.Domain;

namespace Domain;

public class Trade {
    public Guid Id { get; } = Guid.NewGuid();
    public string TraderId { get; }
    public string Instrument { get; }
    public TradeSide Side { get; }
    public decimal Quantity { get; }
    public decimal? Price { get; }
    public OrderType OrderType { get; }
    public TimeInForce TimeInForce { get; }
    public DateTime SubmittedAt { get; } = DateTime.UtcNow;
    public string? StrategyCode { get; }
    public string? PortfolioCode { get; }
    public string? UserComment { get; }
    public DateTime? ExecutionRequestedForUtc { get; }
    public TradeStatus Status { get; private set; } = TradeStatus.Submitted;

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
    private readonly List<IDomainEvent> _domainEvents = new();

    public Trade(
        string traderId,
        string instrument,
        TradeSide side,
        decimal quantity,
        decimal? price,
        OrderType orderType,
        TimeInForce timeInForce,
        string? strategyCode,
        string? portfolioCode,
        string? userComment,
        DateTime? executionRequestedForUtc) {
       
        if (string.IsNullOrWhiteSpace(traderId))
            throw new ArgumentException("TraderId is required");
        if (string.IsNullOrWhiteSpace(instrument))
            throw new ArgumentException("Instrument is required");
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be > 0");
        if (orderType != OrderType.Market && (!price.HasValue || price <= 0))
            throw new ArgumentException("Price required and > 0 for non-market orders");
        if (orderType == OrderType.Market && price.HasValue)
            throw new ArgumentException("Price not allowed for market orders");

        TraderId = traderId;
        Instrument = instrument;
        Side = side;
        Quantity = quantity;
        Price = price;
        OrderType = orderType;
        TimeInForce = timeInForce;
        StrategyCode = strategyCode;
        PortfolioCode = portfolioCode;
        UserComment = userComment;
        ExecutionRequestedForUtc = executionRequestedForUtc;

        _domainEvents.Add(new TradePlaced(Id));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

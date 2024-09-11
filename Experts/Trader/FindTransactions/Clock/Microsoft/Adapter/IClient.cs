namespace Experts.Trader.FindTransactions.Clock.Microsoft.Adapter;

public interface IClient {
    DateTime Now { get; }
}
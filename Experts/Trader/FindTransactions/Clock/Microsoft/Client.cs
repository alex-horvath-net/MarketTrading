namespace Experts.Trader.FindTransactions.Clock.Microsoft;

public class Client : Adapter.IClient
{
    public DateTime Now => DateTime.Now;
}

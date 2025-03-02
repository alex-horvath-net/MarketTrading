using YourBank.Business.Experts.Trader.FindTransactions.Feature.OutputPorts;

namespace YourBank.Business.Experts.Trader.FindTransactions.Clock;

public class DefaultClockAdapter(IClock client) : IClockAdapter {
    public DateTime GetTime() => client.Now;
}

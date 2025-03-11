using Business.Experts.Trader.FindTransactions.Feature.OutputPorts;

namespace Business.Experts.Trader.FindTransactions.Clock;

public class DefaultClockAdapter(IClock client) : IClockAdapter {
    public DateTime GetTime() => client.Now;
}

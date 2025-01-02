using DomainExperts.Trader.FindTransactions.Feature.OutputPorts;

namespace DomainExperts.Trader.FindTransactions.Clock;

public class DefaultClockAdapter(IClock client) : IClockAdapter {
    public DateTime GetTime() => client.Now;
}

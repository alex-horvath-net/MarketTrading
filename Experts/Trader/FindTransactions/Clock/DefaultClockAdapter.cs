using BusinesActors.Trader.FindTransactions.Feature.OutputPorts;

namespace BusinesActors.Trader.FindTransactions.Clock;

public class DefaultClockAdapter(IClock client) : IClockAdapter {
    public DateTime GetTime() => client.Now;
}

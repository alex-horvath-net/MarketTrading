namespace DomainExperts.Trader.FindTransactions.Clock;

public class DefaultClockAdapter(IClock client) : Feature.OutputPorts.IClockAdapter {
    public DateTime GetTime() => client.Now;
}

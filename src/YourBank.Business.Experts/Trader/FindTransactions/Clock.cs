namespace Business.Experts.Trader.FindTransactions;
public class Clock {
    public class Adapter(Adapter.IInfrastructure infra) : Featrure.IClock {
        public DateTime GetTime() => infra.Now;

        public interface IInfrastructure { DateTime Now { get; } }
    }

    public class Infrastructure : Adapter.IInfrastructure {
        public DateTime Now => DateTime.Now;
    }
}

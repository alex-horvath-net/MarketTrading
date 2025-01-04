namespace BusinesActors.Trader.FindTransactions.Triggers.Blazor.InputPort;

public interface ITrigger {
    Task<ViewModel> Execute(string name, string userId, CancellationToken token);
}






namespace Experts.Trader.FindTransactions.BlazorView.Adapter;

public interface IBlazorView {
    Task<ViewModel> Execute(string name, string? userId, CancellationToken token);
}


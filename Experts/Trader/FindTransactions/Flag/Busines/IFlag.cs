namespace Experts.Trader.FindTransactions.Flag.Busines;

public interface IFlag { bool IsPublic(Request request, CancellationToken token); }

using Common.Business.Model;

namespace Experts.Trader.ReadTransactions.Business;

public class Response {
    public Request Request { get; set; }
    public List<string> Errors { get; set; } = [];
    public List<TransactionBM> Transactions { get; set; } = [];
}

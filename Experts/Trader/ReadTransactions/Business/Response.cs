using Common.Business;

namespace Experts.Trader.ReadTransactions.Business;

public class Response {
    public Request Request { get; set; }
    public List<Transaction> Transactions { get; set; }
}

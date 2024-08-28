using Common.Business;

namespace Experts.Trader.ReadTransactions;

public interface IResponse { }
public class Response :IResponse {
    public Request Request { get; set; }
    public List<string> Errors { get; set; } = [];
    public List<Transaction> Transactions { get; set; } = [];
}

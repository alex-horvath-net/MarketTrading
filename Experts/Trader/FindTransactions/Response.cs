using Common.Data.Business.Model;
using Common.Valdation.Business.Model;

namespace Experts.Trader.FindTransactions;

public class Response {
    public Request? Request { get; set; }
    public List<Error> Errors { get; set; } = [];
    public List<Transaction> Transactions { get; set; } = [];
}

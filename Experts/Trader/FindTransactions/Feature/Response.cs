using Infrastructure.Business.Model;
using Infrastructure.Validation.Business.Model;

namespace Experts.Trader.FindTransactions.Feature;

public class Response {
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsUnderConstruction { get; set; } = false;
    public DateTime? StopedAt { get; set; }
    public DateTime? FailedAt { get; internal set; }
    public Exception? Exception { get; set; }
    public Request? Request { get; set; }
    public List<Error> Errors { get; set; } = [];
    public List<Trade> Transactions { get; set; } = [];
}


using Common.Business.Model;
using Common.Validation.Business.Model;

namespace Experts.Trader.EditTransaction;

public class Response {
    public Request? Request { get; set; }
    public List<Error> Errors { get; set; } = [];
    public Transaction? Transaction { get; set; } 
}

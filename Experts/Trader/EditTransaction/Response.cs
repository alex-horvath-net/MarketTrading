using Common.Data.Business.Model;
using Common.Valdation.Business.Model;

namespace Experts.Trader.EditTransaction;

public class Response {
    public Request? Request { get; set; }
    public List<Error> Errors { get; set; } = [];
    public Transaction? Transaction { get; set; } 
}

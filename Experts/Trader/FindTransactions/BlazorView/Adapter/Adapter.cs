using Common.Business.Model;
using Common.Validation.Business.Model;

namespace Experts.Trader.FindTransactions.BlazorView.Adapter;

public class Adapter(Service service) : IBlazorView {

    public async Task<ViewModel> Execute(string name, string? userId, CancellationToken token) {
        var request = new Request {
            Name = name,
            UserId = userId
        };
        var response = await service.Execute(request, token);
        var viewModel = new ViewModel() {
            Meta = ToViewModel(response.Request),
            Errors = response.Errors.Select(ToViewModel).ToList(),
            Transactions = response.Transactions.Select(ToViewModel).ToList()
        };
        return viewModel;
    }

    private static MetaVM ToViewModel(Request businessModel) => new() {
        Id = businessModel.Id,
    };

    private static TransactionVM ToViewModel(Transaction businessModel) => new() {
        Id = businessModel.Id,
        Name = businessModel.Name
    };

    private static ErrorVM ToViewModel(Error businessModel) => new() {
        Name = businessModel.Name,
        Message = businessModel.Message,

    };
}


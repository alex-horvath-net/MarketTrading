using Experts.Trader.FindTransactions.Feature;
using Experts.Trader.FindTransactions.Triggers.Blazor.InputPort;
using Infrastructure.Business.Model;
using Infrastructure.Validation.Business.Model;

namespace Experts.Trader.FindTransactions.Triggers.Blazor;

public class Trigger(IFeature service) : ITrigger {
    public async Task<ViewModel> Execute(string name, string userId, CancellationToken token) {
        var request = new Request {
            Name = name,
            UserId = userId
        };

        token.ThrowIfCancellationRequested();

        var response = await service.Execute(request, token);

        var viewModel = new ViewModel();

        viewModel.Meta = ToMetaViewModel(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorViewModel).ToList();
        viewModel.Transactions = new();
        viewModel.Transactions.Rows = response.Transactions.Select(ToTranaztionViewModel).ToList();
        viewModel.Transactions.Columns.Add(x => x.Id);
        viewModel.Transactions.Columns.Add(x => x.Name);

        token.ThrowIfCancellationRequested();

        return viewModel;

        static ViewModel.MetaVM ToMetaViewModel(Request businessModel) => new() {
            Id = businessModel.Id,
        };

        static ViewModel.TransactionVM ToTranaztionViewModel(Trade businessModel) => new() {
            Id = businessModel.Id,
            Name = businessModel.Name
        };

        static ViewModel.ErrorVM ToErrorViewModel(Error businessModel) => new() {
            Name = businessModel.Name,
            Message = businessModel.Message
        };
    }
}

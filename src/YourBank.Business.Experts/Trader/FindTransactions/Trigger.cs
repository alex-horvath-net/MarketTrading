using System.ComponentModel;
using Business.Domain;
using Infrastructure.Adapters.Blazor;

namespace Business.Experts.Trader.FindTransactions;

public interface ITrigger {
    Task<ViewModel> Execute(string name, string userId, CancellationToken token);
}

public record ViewModel {
    public MetaVM Meta { get; set; }
    public List<ErrorVM> Errors { get; set; } = [];
    public DataListModel<TransactionVM> Transactions { get; set; }
    public class MetaVM {
        public Guid Id { get; internal set; }
    }

    public class ErrorVM {
        public string Name { get; internal set; }
        public string Message { get; internal set; }
    }

    public record TransactionVM {
        [DisplayName("ID")]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}

public class FeatureAdapter(IFindTransactions service) : ITrigger {
    public async Task<ViewModel> Execute(string name, string userId, CancellationToken token) {
        var request = new FindTransactionsRequest {
            Name = name,
            UserId = userId
        };

        token.ThrowIfCancellationRequested();

        var response = await service.Execute(request, token);

        var viewModel = new ViewModel();

        viewModel.Meta = ToMetaVM(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorVM).ToList();
        viewModel.Transactions = new();
        viewModel.Transactions.Rows = response.Transactions.Select(ToTranasactionVM).ToList();
        viewModel.Transactions.Columns.Add(x => x.Id);
        viewModel.Transactions.Columns.Add(x => x.Name);

        token.ThrowIfCancellationRequested();

        return viewModel;

        static ViewModel.MetaVM ToMetaVM(FindTransactionsRequest x) =>
            new() { Id = x.Id, };

        static ViewModel.TransactionVM ToTranasactionVM(Trade x) =>
            new() { Id = x.Id, Name = x.Name };

        static ViewModel.ErrorVM ToErrorVM(Domain.Error x) =>
            new() { Name = x.Name, Message = x.Message };
    }
}

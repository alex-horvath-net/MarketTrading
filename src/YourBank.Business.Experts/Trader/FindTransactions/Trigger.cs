using System.ComponentModel;
using Business.Domain;
using Infrastructure.Adapters.Blazor;
using Infrastructure.Validation.Business.Model;

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

public class Trigger(IFeature service) : ITrigger {
    public async Task<ViewModel> Execute(string name, string userId, CancellationToken token) {
        var request = new Featrure. Request {
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

        static ViewModel.MetaVM ToMetaViewModel(Featrure. Request businessModel) => new() {
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

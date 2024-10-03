using System.ComponentModel;
using Common.Adapters.Blazor;
using Common.Business.Model;
using Common.Validation.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public interface IServiceClient {
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

public class ServiceClient(IService service) : IServiceClient {
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

        static ViewModel.TransactionVM ToTranaztionViewModel(Transaction businessModel) => new() {
            Id = businessModel.Id,
            Name = businessModel.Name
        };

        static ViewModel.ErrorVM ToErrorViewModel(Error businessModel) => new() {
            Name = businessModel.Name,
            Message = businessModel.Message
        };
    }
}

public static class ViewExtensions {
    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IServiceClient, ServiceClient>()
        .AddService(configuration);
}

using System.ComponentModel;
using System.Linq.Expressions;
using Common.Business.Model;
using Common.Validation.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public interface IServiceClient {
    Task<ViewModel> Execute(RequestModel requestModel, CancellationToken token);
}

public record RequestModel(string name, string? userId);
public record ViewModel {
    public MetaVM Meta { get; set; }
    public List<ErrorVM> Errors { get; set; } = [];
    public List<TransactionVM> Transactions { get; set; } = [];
    public List<Expression<Func<TransactionVM, object>>> TransationColumns { get; set; } = [];

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
    public async Task<ViewModel> Execute(RequestModel requestModel, CancellationToken token) {
        var request = new Request {
            Name = requestModel.name,
            UserId = requestModel.userId
        };
        var response = await service.Execute(request, token);
        
        var viewModel = new ViewModel();
       
        viewModel.Meta = ToMetaViewModel(response.Request);
        viewModel.Errors = response.Errors.Select(ToErrorViewModel).ToList();
        viewModel.Transactions = response.Transactions.Select(ToTranaztionViewModel).ToList();
        viewModel.TransationColumns.Add(x => x.Id);
        viewModel.TransationColumns.Add(x => x.Name);

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

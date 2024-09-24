using System.ComponentModel;
using Common.Business.Model;
using Common.Validation.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public interface IView
{
    Task<ViewModel> Execute(RequestModel requestModel, CancellationToken token);
}

public record RequestModel(string name, string? userId);
public record ViewModel {
    public MetaVM Meta { get; set; }
    public List<ErrorVM> Errors { get; set; } = [];
    public List<TransactionVM> Transactions { get; set; } = [];

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

public class View(IService service) : IView
{

    public async Task<ViewModel> Execute(RequestModel requestModel, CancellationToken token)
    {
        var request = new Request
        {
            Name = requestModel.name,
            UserId = requestModel.userId
        };
        var response = await service.Execute(request, token);
        var viewModel = new ViewModel()
        {
            Meta = ToMetaViewModel(response.Request),
            Errors = response.Errors.Select(ToErrorViewModel).ToList(),
            Transactions = response.Transactions.Select(ToTranaztionViewModel).ToList()
        };
        return viewModel;

        static ViewModel.MetaVM ToMetaViewModel(Request businessModel) => new()
        {
            Id = businessModel.Id,
        };

        static ViewModel.TransactionVM ToTranaztionViewModel(Transaction businessModel) => new()
        {
            Id = businessModel.Id,
            Name = businessModel.Name
        };

        static ViewModel.ErrorVM ToErrorViewModel(Error businessModel) => new()
        {
            Name = businessModel.Name,
            Message = businessModel.Message,

        };
    }

}

public static class ViewExtensions
{
    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IView, View>() 
        .AddService(configuration);
}

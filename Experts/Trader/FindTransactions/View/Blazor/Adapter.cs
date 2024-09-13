using System.ComponentModel;
using Common.Business.Model;
using Common.Validation.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.View.Blazor;

public interface IBlazorView
{
    Task<Adapter.ViewModel> Execute(string name, string? userId, CancellationToken token);
}

public class Adapter(Service service) : IBlazorView
{

    public async Task<ViewModel> Execute(string name, string? userId, CancellationToken token)
    {
        var request = new Service.Request
        {
            Name = name,
            UserId = userId
        };
        var response = await service.Execute(request, token);
        var viewModel = new ViewModel()
        {
            Meta = ToMetaViewModel(response.Request),
            Errors = response.Errors.Select(ToErrorViewModel).ToList(),
            Transactions = response.Transactions.Select(ToTranaztionViewModel).ToList()
        };
        return viewModel;

        static MetaVM ToMetaViewModel(Service.Request businessModel) => new()
        {
            Id = businessModel.Id,
        };

        static TransactionVM ToTranaztionViewModel(Transaction businessModel) => new()
        {
            Id = businessModel.Id,
            Name = businessModel.Name
        };

        static ErrorVM ToErrorViewModel(Error businessModel) => new()
        {
            Name = businessModel.Name,
            Message = businessModel.Message,

        };
    }

    public record ViewModel
    {
        public MetaVM Meta { get; set; }
        public List<ErrorVM> Errors { get; set; } = [];
        public List<TransactionVM> Transactions { get; set; } = [];
    }

    public class MetaVM
    {
        public Guid Id { get; internal set; }
    }

    public class ErrorVM
    {
        public string Name { get; internal set; }
        public string Message { get; internal set; }
    }

    public record TransactionVM
    {
        [DisplayName("ID")]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}

public static class AdapterExtensions {

    public static IServiceCollection AddViewAdapter(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IBlazorView, Adapter>()
        .AddFindTransactions(configuration);
}

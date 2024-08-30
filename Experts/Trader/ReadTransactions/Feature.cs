using Common.Business;
using Common.Technology.AppData;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.ReadTransactions;

public class Feature(
    Feature.IValidatorAdapterPort Validator,
    Feature.IRepositoryAdapterPort Repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();

        response.Request = request;

        response.Errors = await Validator.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transactions = await Repository.ReadTransaction(request, token);

        return response;
    }

    public class Request {
        public string Name { get; set; }
    }
   
    public class Response  {
        public Request Request { get; set; }
        public List<string> Errors { get; set; } = [];
        public List<Transaction> Transactions { get; set; } = [];
    }

    public interface IValidatorAdapterPort {
        public Task<List<string>> Validate(Request request, CancellationToken token);
    }

    public interface IRepositoryAdapterPort {
        public Task<List<Common.Business.Transaction>> ReadTransaction(Request request, CancellationToken token);
    }
}

public static class Extensions {
    public static IServiceCollection AddReadTransactions(this IServiceCollection services) {
        services
            .AddScoped<Feature>()
            
            .AddScoped<Feature.IValidatorAdapterPort, ValidatorAdapterPlugin>()
            .AddScoped<ValidatorAdapterPlugin.ValidatorTechnologyPort, ValidatorTechnologyPlugin>()
            .AddScoped<IValidator<Feature.Request>, ValidatorTechnologyPlugin.RequestValidator>()
            
            .AddScoped<Feature.IRepositoryAdapterPort, RepositoryAdapterPlugin>()
            .AddScoped<RepositoryAdapterPlugin.RepositoryTechnologyPort, RepositoryTechnologyPlugin>()
            .AddDbContext<AppDbContext>();
        return services;
    }
}

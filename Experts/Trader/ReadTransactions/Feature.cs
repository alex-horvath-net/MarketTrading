using Common.Business;
using Common.Technology.AppData;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.ReadTransactions;

public class Feature(
    Feature.IValidatorAdapterPort validator,
    Feature.IRepositoryAdapterPort repository) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();

        response.Request = request;

        response.Errors = await validator.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transactions = await repository.ReadTransaction(request, token);

        return response;
    }

    public class Request {
        public string Name { get; set; }
    }

    public class Response {
        public Request Request { get; set; }
        public List<string> Errors { get; set; } = [];
        public List<TransactionBM> Transactions { get; set; } = [];
    }

    public interface IValidatorAdapterPort {
        public Task<List<string>> Validate(Request request, CancellationToken token);
    }

    public interface IRepositoryAdapterPort {
        public Task<List<Common.Business.TransactionBM>> ReadTransaction(Request request, CancellationToken token);
    }
}

public static class Extensions {
    public static IServiceCollection AddReadTransactions(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddScoped<Feature>();
        services.AddValidation();
        services.AddRead(configuration);
        return services;
    }
}
  
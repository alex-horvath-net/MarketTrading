using Common.Business.Model;
using Experts.Trader.FindTransactions.Read;
using Experts.Trader.FindTransactions.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public class Service(Validation.Business validate, Read.Business read) {
    public async Task<Response> Execute(Request request, CancellationToken token) {
        var response = new Response();

        response.Request = request;

        response.Errors = await validate.Validate(request, token);
        if (response.Errors.Count > 0)
            return response;

        response.Transactions = await read.ReadTransaction(request, token);

        return response;
    }
}


public class Request {
    public string Name { get; set; }
}


public class Response {
    public Request Request { get; set; }
    public List<Error> Errors { get; set; } = [];
    public List<Transaction> Transactions { get; set; } = [];
}


public static class Extensions {
    public static IServiceCollection AddReadTransactions(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddScoped<Service>();
        services.AddValidation();
        services.AddRead(configuration);
        return services;
    }
}
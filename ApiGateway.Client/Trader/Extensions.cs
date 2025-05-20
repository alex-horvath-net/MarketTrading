using Microsoft.Extensions.DependencyInjection;


namespace ApiGateway.Client;

public static class Extensions {
    public static IServiceCollection AddTraderApiClient(this IServiceCollection services) {
        services.AddHttpClient<ITraderApiClient, TraderApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001"); // API Gateway base URL
        });
        return services;
    }
}

using IdentityService.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ApiGateway.Client.Trader;

public static class Extensions {
    public static IServiceCollection AddApiGatewayClient(this IServiceCollection services, IConfiguration config) {

        var options = config.GetSection("TraderService").Get<TraderServiceClienOptions>() ?? new TraderServiceClienOptions();

        var clientBuilder = services.AddHttpClient<ITraderServiceClient, TraderServiceClient>(client => {
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        clientBuilder.AddHttpMessageHandler<AccessTokenHandlerForBlazorServer>();

        return services;
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGateway.Client.Trader.PlaceTrade;

public static class Extensions {

    public static IServiceCollection AddPlaceTrade(this IServiceCollection services, ConfigurationManager config) {

        return services
            .AddScoped<IPlaceTradeClient, PlaceTradeClient>();
    }
}

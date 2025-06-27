using Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Client;

public static class Extensions {
    public static IServiceCollection AddIdentityClient(this IServiceCollection services, IConfiguration config) {
        var options = config.GetSection("IdentityService").Get<IdentityClientOptions>() ?? new IdentityClientOptions();

        var clientBuilder = services.AddHttpClient<IIdentityClient, IdentityClient>(client => {
            client.BaseAddress = new Uri(options.BaseAddress);
        });

        clientBuilder.AddHttpMessageHandler<AccessTokenHandlerForBlazorServer>(); 

        return services;
    }
}

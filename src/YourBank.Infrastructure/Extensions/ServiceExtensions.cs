using Microsoft.Extensions.DependencyInjection;
using YourBank.Infrastructure.Validation.FluentValidator.Adapters;
using YourBank.Infrastructure.Validation.FluentValidator.Technology;

namespace YourBank.Infrastructure.Extensions;
public static class ServiceExtensions {

    public static IServiceCollection AddValidatorClient<TRequest>(this IServiceCollection services) => services
        .AddScoped<ICommonClient<TRequest>, CommonClient<TRequest>>();

}

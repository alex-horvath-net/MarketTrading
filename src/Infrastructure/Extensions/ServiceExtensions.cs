using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Validation.FluentValidator.Adapters;
using Infrastructure.Validation.FluentValidator.Technology;

namespace Infrastructure.Extensions;
public static class ServiceExtensions {

    public static IServiceCollection AddValidatorClient<TRequest>(this IServiceCollection services) => services
        .AddScoped<ICommonClient<TRequest>, CommonClient<TRequest>>();

}

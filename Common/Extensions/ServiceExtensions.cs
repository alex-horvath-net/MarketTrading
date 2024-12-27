using Common.Validation.FluentValidator.Adapters;
using Common.Validation.FluentValidator.Technology;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions;
public static class ServiceExtensions {

    public static IServiceCollection AddValidatorClient<TRequest>(this IServiceCollection services) => services
        .AddScoped<ICommonClient<TRequest>, CommonClient<TRequest>>();

}

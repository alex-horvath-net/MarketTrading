using Common.Validation.FluentValidator.Adapters;
using Common.Validation.FluentValidator.Technology;
using Microsoft.Extensions.DependencyInjection;

namespace Common;
public static class Extensions {

    public static IServiceCollection AddValidatorClient<TRequest>(this IServiceCollection services) => services
        .AddScoped<ICommonClient<TRequest>, CommonClient<TRequest>>();

}

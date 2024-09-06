using Common.Valdation.Adapters.Fluentvalidation;
using Common.Valdation.Technology.FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Valdation
{
    public static class Extensions
    {
        public static IServiceCollection AddValidatorClient<TRequest>(this IServiceCollection services) => services
            .AddScoped<IValidatorClient<TRequest>, ValidatorClient<TRequest>>();
    }

}

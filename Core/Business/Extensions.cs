using Core.Solutions.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Business;

public static class Extensions {
    public static IServiceCollection AddStory(this IServiceCollection services) {
        services.AddScoped(typeof(IStory<,>), typeof(Story<,>));

        services.AddScoped(typeof(IValidation<>), typeof(ValidationCore<>));
        return services;
    }
}

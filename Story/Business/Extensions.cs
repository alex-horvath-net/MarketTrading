using Common.Solutions.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Business;

public static class Extensions {
    public static IServiceCollection AddStory(this IServiceCollection services) {
        services.AddScoped(typeof(IStory<,>), typeof(Story<,>));
        
        services.AddScoped(typeof(IValidation<>), typeof(FluentValidator<>));
        return services;
    }
}

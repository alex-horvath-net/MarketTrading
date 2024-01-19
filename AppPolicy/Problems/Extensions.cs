using Core.Story;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Problems;

public static class Extensions {
    public static IServiceCollection AddProblems(this IServiceCollection services) {
        services.AddScoped(typeof(Started<,>));
        services.AddScoped(typeof(FeatureEnabled<,>));

        services.AddScoped(typeof(Completed<,>));

        return services;
    }
}

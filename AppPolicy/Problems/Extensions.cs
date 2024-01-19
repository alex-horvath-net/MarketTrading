using Core.Story;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Problems;

public static class Extensions {
    public static IServiceCollection AddFeatureTask(this IServiceCollection services) {
        services.AddScoped(typeof(IProblem<,>), typeof(Started<,>));
        services.AddScoped(typeof(IProblem<,>), typeof(FeatureEnabled<,>));

        services.AddScoped(typeof(IProblem<,>), typeof(Completed<,>));

        return services;
    }
}

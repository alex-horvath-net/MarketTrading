using Core.Story;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ExpertTasks;

public static class Extensions {
    public static IServiceCollection AddFeatureTask(this IServiceCollection services) {
        services.AddScoped(typeof(IProblem<,>), typeof(SrartTask<,>));
        services.AddScoped(typeof(IProblem<,>), typeof(FeatureTask<,>));

        services.AddScoped(typeof(IProblem<,>), typeof(EndTask<,>));

        return services;
    }
}

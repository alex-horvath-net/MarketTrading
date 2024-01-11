using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Core.UserTasks;

public static class Extensions {
    public static IServiceCollection AddFeatureTask(this IServiceCollection services) {
        services.AddScoped(typeof(IScope<,>), typeof(SrartTask<,>));
        services.AddScoped(typeof(IScope<,>), typeof(FeatureTask<,>));

        services.AddScoped(typeof(IScope<,>), typeof(EndTask<,>));

        return services;
    }
}

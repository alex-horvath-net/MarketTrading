using AppPolicy.UserStory;
using AppPolicy.UserTasks;
using Microsoft.Extensions.DependencyInjection;

namespace AppPolicy;

public static class Extensions {
    public static IServiceCollection AddCoreSystem(this IServiceCollection services) {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}
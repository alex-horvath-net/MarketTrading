using Core.ExpertTasks;
using Core.Story;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Extensions {
    public static IServiceCollection AddCoreSystem(this IServiceCollection services) {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}
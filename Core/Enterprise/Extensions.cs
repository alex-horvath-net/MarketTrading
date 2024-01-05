using Core.Enterprise.UserStory;
using Core.Enterprise.UserTasks;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Enterprise;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}
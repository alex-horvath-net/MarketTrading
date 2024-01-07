using Core.Sys.UserStory;
using Core.Sys.UserTasks;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Sys;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}
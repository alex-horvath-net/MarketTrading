using Core.UserStory;
using Core.UserTasks;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Extensions
{
    public static Task<T> ToTask<T>(this T value) => Task.FromResult(value);

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}

using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Core.UserTasks;

public static class Extensions
{
    public static IServiceCollection AddFeatureTask(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserTask<,>), typeof(FeatureTask<,>));

        return services;
    }
}

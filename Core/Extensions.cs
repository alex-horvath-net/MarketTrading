using Core.Tasks;
using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserStory<,>), typeof(UserStoryCore<,>));
        services.AddScoped(typeof(ITask<,>), typeof(FeatureTask<,>));

        return services;
    }
}

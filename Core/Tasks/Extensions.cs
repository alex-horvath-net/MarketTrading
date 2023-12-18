using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Tasks;

public static class Extensions
{
    public static IServiceCollection AddFeatureTask(this IServiceCollection services)
    {
        services.AddScoped(typeof(ITask<,>), typeof(FeatureTask<,>));

        return services;
    }
}

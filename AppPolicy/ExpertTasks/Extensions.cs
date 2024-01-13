using Core.ExpertStory;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ExpertTasks;

public static class Extensions {
    public static IServiceCollection AddFeatureTask(this IServiceCollection services) {
        services.AddScoped(typeof(IExpertTask<,>), typeof(SrartTask<,>));
        services.AddScoped(typeof(IExpertTask<,>), typeof(FeatureTask<,>));

        services.AddScoped(typeof(IExpertTask<,>), typeof(EndTask<,>));

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Story.Problems;

namespace Story;

public static class Extensions {
    public static IServiceCollection AddStory(this IServiceCollection services) {
        services.AddScoped(typeof(IStory<,>), typeof(Story<,>));
        
        services.AddScoped(typeof(Started<,>));
        services.AddScoped(typeof(FeatureEnabled<,>));
        services.AddScoped(typeof(Completed<,>));

        return services;
    }
}

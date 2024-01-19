using Microsoft.Extensions.DependencyInjection;

namespace Core.Story;

public static class Extensions {
    public static IServiceCollection AddStory(this IServiceCollection services) {
        services.AddScoped(typeof(IStory<,>), typeof(Story<,>));

        return services;
    }
}

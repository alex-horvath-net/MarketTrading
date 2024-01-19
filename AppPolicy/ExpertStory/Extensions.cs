using Microsoft.Extensions.DependencyInjection;

namespace Core.ExpertStory;

public static class Extensions {
    public static IServiceCollection AddUserStory(this IServiceCollection services) {
        services.AddScoped(typeof(IExpertStory<,>), typeof(Story<,>));

        return services;
    }
}

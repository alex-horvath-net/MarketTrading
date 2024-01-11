using Microsoft.Extensions.DependencyInjection;

namespace AppPolicy.UserStory;

public static class Extensions {
    public static IServiceCollection AddUserStory(this IServiceCollection services) {
        services.AddScoped(typeof(IUserStory<,>), typeof(UserStory<,>));

        return services;
    }
}

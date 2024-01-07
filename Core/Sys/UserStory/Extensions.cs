using Microsoft.Extensions.DependencyInjection;

namespace Core.Sys.UserStory;

public static class Extensions
{
    public static IServiceCollection AddUserStory(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserStory<,>), typeof(UserStory<,>));

        return services;
    }
}

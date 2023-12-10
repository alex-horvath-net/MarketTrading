using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.UserStory.UserStoryUnit;

public static class Extensions
{
    public static IServiceCollection AddUserStory(this IServiceCollection services)
    {
        services.AddScoped<IUserStory, UserStory>();
        return services;                                              
    }
}

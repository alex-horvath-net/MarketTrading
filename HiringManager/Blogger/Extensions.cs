using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.UserStories.ReadPosts;

namespace Users.Blogger;

public static class Extensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services)
    {
        services.AddReadPostsUserStory();

        return services;
    }
}

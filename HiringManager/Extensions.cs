using Blogger.UserStories.ReadPosts;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger;

public static class Extensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services)
    {
        services.AddReadPostsUserStory();

        return services;
    }
}

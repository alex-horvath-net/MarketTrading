using Blogger.UserStories.ReadPosts.Tasks.ReadTask;
using Blogger.UserStories.ReadPosts.Tasks.ValidationTask;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.UserStories.ReadPosts;

public static class Extensions
{
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services)
    {
        services.AddReadTask();
        services.AddValidationTask();
         
        return services;
    }
}

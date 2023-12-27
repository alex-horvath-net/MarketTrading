using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask;

namespace Users.Blogger.UserStories.ReadPosts;

public static class Extensions
{
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services)
    {
        services.AddReadTask();
        services.AddValidationTask();

        return services;
    }
}

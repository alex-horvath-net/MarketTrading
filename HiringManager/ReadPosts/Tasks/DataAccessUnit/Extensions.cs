using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Tasks.DataAccessUnit;

public static class Extensions
{
    public static IServiceCollection AddReadPostsTask(this IServiceCollection services)
    {
        services.AddScoped<ITask, ReadPostsTask>();
        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadTask;
using Users.Blogger.ReadPostsUserStory.ValidationTask;

namespace Users.Blogger.ReadPostsUserStory;

public static class Extensions
{
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services)
    {
        services.AddReadTask();
        services.AddValidationTask();

        return services;
    }
}

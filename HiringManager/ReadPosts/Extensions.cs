using Blogger.ReadPosts.Adapters.DataAccessUnit;
using Blogger.ReadPosts.Adapters.ValidationUnit;
using Blogger.ReadPosts.Plugins.DataAccessUnit;
using Blogger.ReadPosts.Plugins.ValidationUnit;
using Blogger.ReadPosts.Tasks.DataAccessUnit;
using Blogger.ReadPosts.Tasks.ValidationUnit;
using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts;

public static class Extensions
{
    public static IServiceCollection AddReadPosts(this IServiceCollection services)
    {
        services.AddUserStory();

        services.AddValidationTask();
        services.AddReadPostsTask();

        services.AddValidationAdapter();
        services.AddDataAccessAdapter();

        services.AddDataAccessPlugin();
        services.AddValidationPlugin();

        return services;
    }
}

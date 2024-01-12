using BusinessExperts.Blogger.ReadPostsExpertStory;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExperts.Blogger;

public static class Extensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddReadPostsUserStory();
}

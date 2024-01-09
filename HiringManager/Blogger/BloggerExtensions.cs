using Experts.Blogger.ReadPostsUserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger;

public static class BloggerExtensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddReadPostsUserStory();
}

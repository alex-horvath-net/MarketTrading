using Experts.Blogger.ReadPostsUserStory;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.Extensions;

public static class BloggerExtensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddReadPostsUserStory();
}

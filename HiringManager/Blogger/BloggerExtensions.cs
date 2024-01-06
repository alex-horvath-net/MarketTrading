using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory;

namespace Users.Blogger;

public static class BloggerExtensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddReadPostsUserStory(); 
}

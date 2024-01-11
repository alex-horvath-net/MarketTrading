using BusinessExperts.Blogger.ReadPostsExpertStory;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExperts.Blogger.Extensions;

public static class BloggerExtensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddReadPostsUserStory();
}

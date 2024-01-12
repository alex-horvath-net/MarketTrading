using BusinessExperts.Blogger.ReadPosts;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExperts.Blogger;

public record Expert(
    ExpertStrory ReadPosts);


public static class Extensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddReadPostsUserStory();
}


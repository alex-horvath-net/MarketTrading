using Experts.Blogger.ReadPosts;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger;

public record Expert(
    ReadPosts.IFeature ReadPosts);


public static class Extensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddReadPosts();
}
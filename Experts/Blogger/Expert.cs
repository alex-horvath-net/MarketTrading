
using Core.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger;

public record Expert(
     StoryCore<ReadPosts.Request, ReadPosts.Response> ReadPosts
    );



public static class Extensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddScoped<StoryCore<ReadPosts.Request, ReadPosts.Response>, ReadPosts.Story>()
        .AddScoped<IValidation<ReadPosts.Request>, ReadPosts.Validation>()
        .AddScoped<ReadPosts.IRepository, ReadPosts.Repository>();

}


using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger;

public record Expert(
     Common.Business.Story<ReadPosts.Story.Request, ReadPosts.Story.Response> ReadPosts
    );



public static class Extensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddScoped<Common.Business.Story<ReadPosts.Story.Request, ReadPosts.Story.Response>, ReadPosts.Story>()
        .AddScoped<Common.Business.IValidation<ReadPosts.Story.Request>, ReadPosts.Validation>()
        .AddScoped<ReadPosts.Story.IRepository, ReadPosts.Repository>();

}

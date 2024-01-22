
using Core.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger;

public record Expert(
     Story<ReadPosts.Story.Request, ReadPosts.Story.Response> ReadPosts
    );



public static class Extensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddScoped<Story<ReadPosts.Story.Request, ReadPosts.Story.Response>, ReadPosts.Story>()
        .AddScoped<IValidation<ReadPosts.Story.Request>, ReadPosts.Validation>()
        .AddScoped<ReadPosts.Story.IRepository, ReadPosts.Repository>();

}

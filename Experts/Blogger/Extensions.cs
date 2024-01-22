using Common.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger;

public static class Extensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddScoped<Story<ReadPosts.Story.Request, ReadPosts.Story.Response>, ReadPosts.Story>()
        .AddScoped<ReadPosts.Story.IValidation, ReadPosts.Validation>()
        .AddScoped<ReadPosts.Story.IRepository, ReadPosts.Repository>()

        ;

}


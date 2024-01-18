using Core.ExpertStory;
using Experts.Blogger.ReadPosts;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger;

public record Expert(ReadPosts.ExpertStrory ReadPosts);


public static class Extensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddScoped<ExpertStory<Request, Response>, ExpertStrory>()
            .AddScoped<IProblem<Request, Response>, ReadPosts.Read.Problem>()
            .AddScoped<ReadPosts.Read.ISolution, ReadPosts.Read.Solution>()

            .AddScoped<IProblem<Request, Response>, ReadPosts.Validation.Problem>()
            .AddScoped<ReadPosts.Validation.ISolution, ReadPosts.Validation.Solution>();
}


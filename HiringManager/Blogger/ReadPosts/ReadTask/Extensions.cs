using Core.ExpertStory;
using Experts.Blogger.ReadPosts;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts.ReadTask;

public static class Extensions {
    public static IServiceCollection AddReadTask(this IServiceCollection services) => services
        .AddScoped<IScope<Request, Response>, Scope>()
        //.AddScoped<ISolutionExpert, SolutionExpert>()
        .AddScoped<ISolution, Solution>();
}
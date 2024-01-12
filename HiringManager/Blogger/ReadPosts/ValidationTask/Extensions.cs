using Core.ExpertStory;
using Experts.Blogger.ReadPosts;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts.ValidationTask;

public static class Extensions {
    public static IServiceCollection AddValidationTask(this IServiceCollection services) => services
        .AddScoped<IScope<Request, Response>, Scope>()
        //.AddScoped<ISolutionExpert, SolutionExpert>()
        .AddScoped<ISolution, Solution>();
}
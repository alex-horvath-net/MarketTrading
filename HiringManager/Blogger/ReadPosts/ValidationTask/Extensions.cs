using BusinessExperts.Blogger.ReadPosts;
using Core.ExpertStory;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExperts.Blogger.ReadPosts.ValidationTask;

public static class Extensions {
    public static IServiceCollection AddValidationTask(this IServiceCollection services) => services
        .AddScoped<IScope<Request, Response>, Scope>()
        //.AddScoped<ISolutionExpert, SolutionExpert>()
        .AddScoped<ISolution, Solution>();
}
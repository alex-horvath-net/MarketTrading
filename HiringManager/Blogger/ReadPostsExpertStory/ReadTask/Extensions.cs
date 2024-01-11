using Core.UserStory;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public static class Extensions
{
    public static IServiceCollection AddReadTask(this IServiceCollection services) => services
        .AddScoped<IScope<Request, Response>, Scope>()
        //.AddScoped<ISolutionExpert, SolutionExpert>()
        .AddScoped<ISolution, Solution>();
} 
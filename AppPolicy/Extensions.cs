using Core.Problems;
using Core.Story;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Extensions {
    public static IServiceCollection AddCoreSystem(this IServiceCollection services) {
        services.AddStory();
        services.AddProblems();

        return services;
    }
}
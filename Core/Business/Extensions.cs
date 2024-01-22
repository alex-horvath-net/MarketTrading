using Core.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Business;

public static class Extensions {
    public static IServiceCollection AddCoreBusiness(this IServiceCollection services) {
        services.AddScoped(typeof(IStory<,,>), typeof(StoryCore<,,>));

        services.AddCoreSolutions(null);

        return services;
    }

    public static bool HasIssue(this IEnumerable<ValidationResult> results) => results.Any(x => x.IsFailed);
}

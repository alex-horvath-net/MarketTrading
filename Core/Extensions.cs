using Core.Business;
using Core.Solutions.Logging;
using Core.Solutions.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Extensions {
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration) => services
        .AddCoreBusiness()
        .AddCoreSolutions(configuration);

    public static IServiceCollection AddCoreBusiness(this IServiceCollection services) {
        services.AddScoped(typeof(IStory<,,>), typeof(StoryCore<,,>));

        services.AddCoreSolutions(null);

        return services;
    }

    public static bool HasIssue(this IEnumerable<ValidationResult> results) => results.Any(x => x.IsFailed);

    public static IServiceCollection AddCoreSolutions(this IServiceCollection services, IConfiguration configuration) {
        services.AddMicrosoftLogger(configuration);
        services.AddFluentValidation();
        return services;
    }

    public static IServiceCollection AddMicrosoftLogger(this IServiceCollection services, IConfiguration configuration) {
        services.AddScoped(typeof(Business.ILogger<>), typeof(MicrosoftLogger<>));
        services.AddLogging(builder => builder.AddConfiguration(configuration));
        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services) {
        services.AddScoped(typeof(IValidator<>), typeof(ValidationCore<>));
        return services;
    }
}

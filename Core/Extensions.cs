using Core.Business;
using Core.Solutions.Logging;
using Core.Solutions.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Extensions {
    public static IServiceCollection AddCore(this IServiceCollection services) => services
        .AddCoreBusiness()
        .AddCoreSolutions()        ;

    public static IServiceCollection AddCoreBusiness(this IServiceCollection services) {
        services.AddScoped(typeof(IUserStoryCore<,>), typeof(StoryCore<,>));
        return services;
    }

    public static IServiceCollection AddCoreSolutions(this IServiceCollection services) {
        services.AddMicrosoftLogger();
        services.AddFluentValidation();
        return services;
    }

    public static IServiceCollection AddMicrosoftLogger(this IServiceCollection services) {
        services.AddScoped(typeof(Business.ILogger<>), typeof(MicrosoftLogger<>));
        services.AddLogging();
        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services) {
        services.AddScoped(typeof(IValidator<>), typeof(ValidationCore<>));
        return services;
    }

    public static bool HasFailed(this IEnumerable<Result> results) => results.Any(x => x.IsFailed);
}

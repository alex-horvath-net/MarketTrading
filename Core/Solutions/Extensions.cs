using Core.Business;
using Core.Solutions.Logging;
using Core.Solutions.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Solutions;
public static class Extensions
{
    public static IServiceCollection AddCoreSolutions(this IServiceCollection services, IConfiguration configuration) {
        services.AddMicrosoftLogger(configuration);
        services.AddFluentValidation();
        return services;
    }

    public static IServiceCollection AddMicrosoftLogger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(Business.ILogger<>), typeof(MicrosoftLogger<>));
        services.AddLogging(builder => builder.AddConfiguration(configuration));
        return services; 
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services) {
        services.AddScoped(typeof(IValidator<>), typeof(ValidationCore<>));
        return services;
    }
} 
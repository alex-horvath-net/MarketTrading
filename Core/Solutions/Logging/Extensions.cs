using Core.Solutions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Solutions.Logging;
public static class Extensions {
  public static IServiceCollection AddMicrosoftLogger(this IServiceCollection services) {
    services.AddScoped(typeof(Core.Business.ILog<>), typeof(MicrosoftLog<>));
    services.AddLogging();
    return services;
  }
}
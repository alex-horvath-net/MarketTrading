using Core.Business;
using Core.Solutions.Time;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Solutions.Time;
public static class Extensions {
  public static IServiceCollection AddMicrosoftTime(this IServiceCollection services) {
    services.AddSingleton<ITime, MicrosoftTime>();
    return services;
  }
}
using Core.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Solutions.Setting;
public static class Extensions {
  public static IServiceCollection AddMicrosoftSettings(this IServiceCollection services) => services
    .AddSingleton(typeof(ISettings<>), typeof(MicrosoftSettings<>));
}
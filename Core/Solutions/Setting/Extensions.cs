using Core.Business;
using Core.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Solutions.Setting;
public static class Extensions {
    public static IServiceCollection AddMicrosoftSettings(this IServiceCollection services) => services
        .AddSingleton(typeof(ISettings<>), typeof(MicrosoftSettings<>));

    public static IServiceCollection AddSettings<TSettings>(this IServiceCollection services, Action<TSettings> settingsBuilder = null) 
        where TSettings : SettingsCore => services
        .AddOptionsWithValidateOnStart<TSettings>()
        .Configure<IConfiguration>((settings, configuration) => {
            configuration.GetSection(settings.SectionName).Bind(settings);
            settingsBuilder ??= _ => { };
            settingsBuilder(settings);
        })
        .ValidateDataAnnotations()
        .Services;
}
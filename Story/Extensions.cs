using Common.Business;
using Common.Solutions.Data.MainDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class Extensions {
    public static IServiceCollection AddCommon(this IServiceCollection services, Action<Common> optionBuilder = null) => services
        .AddOptionsWithValidateOnStart<Common>()
        .Configure<IConfiguration>((options, config) => {
            config.GetSection(Common.SectionName).Bind(options);
            optionBuilder ??= _ => { };
            optionBuilder(options);
        })
        .ValidateDataAnnotations()
        .Services
        .AddCommonBusiness()
        .AddCommonSolutions();
}

using Core.Business;
using Core.Solutions.Logging;
using Core.Solutions.Setting;
using Core.Solutions.Time;
using Core.Solutions.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class Extensions {
    public static IServiceCollection AddCore(this IServiceCollection services) => services
        .AddCoreBusiness()
        .AddCoreSolutions();

    public static IServiceCollection AddCoreBusiness(this IServiceCollection services) => services
        .AddScoped(typeof(IUserStory<,>), typeof(UserStory<,>));


    public static IServiceCollection AddCoreSolutions(this IServiceCollection services) => services
        .AddMicrosoftTime()
        .AddMicrosoftSettings()
        .AddMicrosoftLogger()
        .AddFluentValidation();
}

using Core;
using Core.Business;
using Core.Business.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;
public class FeatureActivationUserWorkStep<TSettings>(ISettings<TSettings> settings) : UserWorkStep<Request, Response> where TSettings : SettingsCore {
    public override Task<bool> Run(Response response, CancellationToken token) {
        response.MetaData.Enabled = settings.Value.Enabled;
        return response.MetaData.Enabled.ToTask();
    }
}


public static class FeatureActivationUserWorkStepExtensions {
    public static IServiceCollection AddFeatureActivationUserWorkStep(this IServiceCollection services) => services
        .AddScoped<IUserWorkStep<Request, Response>, FeatureActivationUserWorkStep<Settings>>();
}

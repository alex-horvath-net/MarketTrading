using Core;
using Core.Business;
using Core.Business.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts.WorkSteps;

public class FeatureSwitch<TSettings>(ISettings<TSettings> settings) : UserWorkStep<UserStoryRequest, UserStoryResponse> where TSettings : SettingsCore {
    public override Task<bool> Run(UserStoryResponse response, CancellationToken token) {
        response.MetaData.Enabled = settings.Value.Enabled;
        return response.MetaData.Enabled.ToTask();
    }
}


public static class FeatureActivationUserWorkStepExtensions {
    public static IServiceCollection AddFeatureActivationUserWorkStep(this IServiceCollection services) => services
        .AddScoped<IUserWorkStep<UserStoryRequest, UserStoryResponse>, FeatureSwitch<UserStorySettings>>();
}

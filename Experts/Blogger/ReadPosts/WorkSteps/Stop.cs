using Core;
using Core.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts.WorkSteps;

public class Stop(ITime time) : UserWorkStep<UserStoryRequest, UserStoryResponse> {
    public override Task<bool> Run(UserStoryResponse response, CancellationToken token) {
        response.MetaData.Stoped = time.Now;
        return true.ToTask();
    }
}

public static class StopUserWorkStepExtensions {
    public static IServiceCollection AddStopUserWorkStep(this IServiceCollection services) => services
        .AddScoped<IUserWorkStep<UserStoryRequest, UserStoryResponse>, Stop>();
}



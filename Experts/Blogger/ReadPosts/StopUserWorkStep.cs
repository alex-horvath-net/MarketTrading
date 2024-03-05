using Core;
using Core.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public class StopUserWorkStep(ITime time) : UserWorkStep<Request, Response> {
    public override Task<bool> Run(Response response, CancellationToken token) {
        response.MetaData.Stoped = time.Now;
        return true.ToTask();
    }
}

public static class StopUserWorkStepExtensions {
    public static IServiceCollection AddStopUserWorkStep(this IServiceCollection services) => services
        .AddScoped<IUserWorkStep<Request, Response>, StopUserWorkStep>();
}



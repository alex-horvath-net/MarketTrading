using Core;
using Core.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public class StartUserWorkStep(ITime time) : UserWorkStep<Request, Response> {
    public override Task<bool> Run(Response response, CancellationToken token) {
        response.MetaData.StartedAt = time.Now;
        return true.ToTask();
    }
}

public static class StartUserWorkStepExtensions {
    public static IServiceCollection AddStartUserWorkStep(this IServiceCollection services) => services
        .AddScoped<IUserWorkStep<Request, Response>, StartUserWorkStep>();
}




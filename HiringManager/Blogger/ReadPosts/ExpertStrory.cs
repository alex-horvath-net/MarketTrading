using Common.Scope.ScopeModel;
using Core.ExpertStory;
using Core.ExpertStory.DomainModel;
using Experts.Blogger.ReadPosts.Read;
using Experts.Blogger.ReadPosts.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public class ExpertStrory(IEnumerable<IExpertTask<Request, Response>> tasks) : ExpertStory<Request, Response>(tasks) {
}


public record Request(string Title, string Content) : Core.ExpertStory.DomainModel.Request {
    public static Request Empty { get; } = new(default, default);
}


public record Response() : Response<Request> {
    public static Response Empty { get; } = new();
    public IEnumerable<Post>? Posts { get; set; }
}


public static class Extensions {
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services) => services
        .AddScoped<ExpertStory<Request, Response>, ExpertStrory>()

        .AddScoped<IExpertTask<Request, Response>, Read.ExpertTask>()
        .AddScoped<Read.ISolution, Read.Solutions.Solution>()

        .AddScoped<IExpertTask<Request, Response>, Validation.ExpertTask>()
        .AddScoped<Validation.ISolution, Validation.Solutions.Solution>();
}
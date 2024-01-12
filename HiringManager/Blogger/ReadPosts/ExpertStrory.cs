using Common.Scope.ScopeModel;
using Core.ExpertStory;
using Core.ExpertStory.DomainModel;
using Experts.Blogger.ReadPosts.ReadTask;
using Experts.Blogger.ReadPosts.ValidationTask;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public class ExpertStrory(IEnumerable<IScope<Request, Response>> tasks) : ExpertStory<Request, Response>(tasks) {
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
        .AddReadTask()
        .AddValidationTask();
}
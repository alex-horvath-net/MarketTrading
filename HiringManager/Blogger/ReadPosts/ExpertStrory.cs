using Common.ExpertStrory.StoryModel;
using Core.ExpertStory;
using Core.ExpertStory.StoryModel;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public class ExpertStrory(IEnumerable<IExpertTask<Request, Response>> tasks) : ExpertStory<Request, Response>(tasks) {
}


public record Request(string Title, string Content) : Core.ExpertStory.StoryModel.Request {
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
        .AddScoped<Read.ISolution, Read.Solution>()

        .AddScoped<IExpertTask<Request, Response>, Validation.ExpertTask>()
        .AddScoped<Validation.ISolution, Validation.Solution>();
}
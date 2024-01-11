using AppCore.UserStory.DomainModel;
using AppPolicy.UserStory.DomainModel;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPostsUserStory;

public record Request(string Title, string Content) : AppPolicy.UserStory.DomainModel.Request;

public record Response() : Response<Request> {
    public List<Post>? Posts { get; set; }
}

public static class UserStroryExtensions
{
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services) => services
        .AddReadTask()
        .AddValidationTask();
}
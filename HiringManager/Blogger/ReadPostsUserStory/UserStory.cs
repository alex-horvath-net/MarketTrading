using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.ReadPostsUserStory;

public record Request(string Title, string Content) : Core.Enterprise.UserStory.Request;


public record Response() : Response<Request>
{
    public List<DomainModel.Post>? Posts { get; set; }
}


public static class UserStroryExtensions
{
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services) => services
        .AddReadTask()
        .AddValidationTask();
}
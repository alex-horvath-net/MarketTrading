using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadUserTask;
using Users.Blogger.ReadPostsUserStory.ValidationUserTask;

namespace Users.Blogger.ReadPostsUserStory;

public record Request(string Title, string Content) : RequestCore;


public record Response() : Response<Request>
{
    public List<DomainModel.Post>? Posts { get; set; }
}


public static class UserStroryExtensions
{
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services) => services
        .AddReadUserTask()
        .AddValidationTask();
}
using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.ReadPostsUserStory;

public record Request(string Title, string Content) : SUS.Request;


public record Response() : SUS.Response<Request>
{
    public List<DomainModel.Post>? Posts { get; set; }
}


public static class UserStroryExtensions
{
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services) => services
        .AddReadTask()
        .AddValidationTask();
}
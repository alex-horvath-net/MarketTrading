using BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;
using Common.UserStory.DomainModel;
using Core.UserStory.DomainModel;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExperts.Blogger.ReadPostsExpertStory;

public record Request(string Title, string Content) : Core.UserStory.DomainModel.Request;

public record Response() : Response<Request> {
    public List<Post>? Posts { get; set; }
}

public static class UserStroryExtensions {
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services) => services
        .AddReadTask()
        .AddValidationTask();
}
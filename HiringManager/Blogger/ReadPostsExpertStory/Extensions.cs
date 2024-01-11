using BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;
using BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExperts.Blogger.ReadPostsExpertStory;

public static class Extensions {
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services) => services
        .AddReadTask()
        .AddValidationTask();
}
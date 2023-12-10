using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Tasks.ValidationUnit;

public static class Extensions
{
    public static IServiceCollection AddValidationTask(this IServiceCollection services)
    {
        services.AddScoped<ITask, ValidationTask>();
        return services;                                                                         
    }
}

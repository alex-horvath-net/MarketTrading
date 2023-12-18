using Core.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Core.UserStory;

public static class Extensions
{
    public static Task<T> ToTask<T>(this T value) => Task.FromResult(value);

    public static IServiceCollection AddUserStory(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserStory<,>), typeof(UserStoryCore<,>));

        return services;
    }
}

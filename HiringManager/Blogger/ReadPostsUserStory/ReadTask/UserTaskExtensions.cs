using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

namespace Users.Blogger.ReadPostsUserStory.ReadTask;

public static class UserTaskExtensions
{
    public static IServiceCollection AddReadTask(this IServiceCollection services)
    {
        services.AddScoped<IUserTask<Request, Response>, UserTask>();
        services.AddDataAccessSocket();

        return services;
    }
}

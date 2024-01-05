using Core.Enterprise.UserStory;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory;
using Users.Blogger.ReadPostsUserStory.UserTasks.ReadTask.Sockets.DataAccessSocket;

namespace Users.Blogger.ReadPostsUserStory.UserTasks.ReadTask;

public static class Extensions
{
    public static IServiceCollection AddReadTask(this IServiceCollection services)
    {
        services.AddScoped<IUserTask<Request, Response>, ReadPostsTask>();
        services.AddDataAccessSocket();

        return services;
    }
}

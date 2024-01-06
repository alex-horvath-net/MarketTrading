using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket;

namespace Users.Blogger.ReadPostsUserStory.ReadUserTask;

public class UserTask(IDataAccessSocket socket) : IUserTask<Request, Response>
{
    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Posts = await socket.Read(response.Request, token);
        return false;
    }
}

public interface IDataAccessSocket
{
    Task<List<DomainModel.Post>> Read(Request request, CancellationToken token);
}

public static class UserTaskExtensions
{
    public static IServiceCollection AddReadUserTask(this IServiceCollection services) => services
        .AddScoped<IUserTask<Request, Response>, UserTask>()
        .AddDataAccessSocket();
}
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

namespace Users.Blogger.ReadPostsUserStory.ReadTask;

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
    public static IServiceCollection AddReadTask(this IServiceCollection services)
    {
        services.AddScoped<IUserTask<Request, Response>, UserTask>();
        services.AddDataAccessSocket();

        return services;
    }
}
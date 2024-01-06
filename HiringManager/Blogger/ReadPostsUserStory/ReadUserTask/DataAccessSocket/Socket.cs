using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadUserTask;
using Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket.DataAccessPlugin;

namespace Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket;

public class Socket(IDataAccessPlugin plugin) : IDataAccessSocket
{
    public async Task<List<DomainModel.Post>> Read(Request request, CancellationToken token)
    {
        var dataModel = await plugin.Read(request.Title, request.Content, token);
        var userStoryDomainModel = dataModel.Select(x => new DomainModel.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return userStoryDomainModel;
    }
}

public interface IDataAccessPlugin
{
    Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token);
}


public static class SocketExtensions
{
    public static IServiceCollection AddDataAccessSocket(this IServiceCollection services) => services
        .AddScoped<IDataAccessSocket, Socket>()
        .AddDataAccessPlugin();
}
using Core.App.Plugins.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPostsUserStory;

public class ReadPlugin(DB entityFramework) : IReadPlugin
{
    public async Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token)
    {
        var pluginModel = await entityFramework
            .Posts
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(token);

        var dataModel = pluginModel;
        return dataModel;
    }
}


public interface IReadPlugin
{
    Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token);
}


public class ReadSocket(IReadPlugin plugin) : IReadSocket
{
    public async Task<List<DomainModel.Post>> Read(Request request, CancellationToken token)
    {
        var dataModel = await plugin.Read(request.Title, request.Content, token);
        var domainModel = dataModel.Select(x => new DomainModel.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return domainModel;
    }
}


public interface IReadSocket
{
    Task<List<DomainModel.Post>> Read(Request request, CancellationToken token);
}


public class ReadTask(IReadSocket socket) : SUS.IUserTask<Request, Response>
{
    public async Task Run(Response response, CancellationToken token) =>
        response.Posts = await socket.Read(response.Request, token);
}


public static class ReadUserExtensions
{
    public static IServiceCollection AddReadTask(this IServiceCollection services) => services
        .AddScoped<SUS.IUserTask<Request, Response>, ReadTask>()
        .AddScoped<IReadSocket, ReadSocket>()
        .AddScoped<IReadPlugin, ReadPlugin>();
}
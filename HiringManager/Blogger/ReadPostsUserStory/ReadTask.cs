using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.ReadPostsUserStory;

public class ReadTask(IReadSocket socket) : SUS.IUserTask<Request, Response>
{
    public async Task Run(Response response, CancellationToken token) =>
        response.Posts = await socket.Read(response.Request, token);
}

public interface IReadSocket
{
    Task<List<DomainModel.Post>> Read(Request request, CancellationToken token);
}

public class ReadSocket(IReadPlugin plugin) : IReadSocket
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

public interface IReadPlugin
{
    Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token);
}


public class ReadPlugin(BlogDbContext db) : IReadPlugin
{
    public async Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token)
    {
        var pluginModel = await db
            .Posts
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(token);

        var dataModel = pluginModel;
        return dataModel;
    }
}

public static class ReadUserExtensions
{
    public static IServiceCollection AddReadTask(this IServiceCollection services) => services
        .AddScoped<SUS.IUserTask<Request, Response>, ReadTask>()
        .AddScoped<IReadSocket, ReadSocket>()
        .AddScoped<IReadPlugin, ReadPlugin>();
}
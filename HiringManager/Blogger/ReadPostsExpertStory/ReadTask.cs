using Common.Plugins.DataAccess;
using Core.UserStory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DataModel = Common.Sockets.DataModel;
using DomainModel = Common.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory;

public class ReadPlugin(DB entityFramework) : IReadPlugin {
    public async Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token) {
        var pluginModel = await entityFramework
            .Posts
            .Include(x=>x.PostTags)
            .ThenInclude(x=>x.Tag)
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(token);

        var dataModel = pluginModel;
        return dataModel;
    }
}


public interface IReadPlugin {
    Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token);
}
public class ReadSocket(IReadPlugin plugin) : IReadSocket {
    public async Task<List<DomainModel.Post>> Read(Request request, CancellationToken token) {
        var dataModel = await plugin.Read(request.Title, request.Content, token);
        var domainModel = dataModel.Select(data => new DomainModel.Post() {
            Title = data.Title,
            Content = data.Content
        }).ToList();
        return domainModel;
    }
}


public interface IReadSocket {
    Task<List<DomainModel.Post>> Read(Request request, CancellationToken token);
}
public class ReadTask(IReadSocket socket) : IScope<Request, Response> {
    public async Task Run(Response response, CancellationToken token) =>
        response.Posts = await socket.Read(response.Request, token);
}


public static class ReadUserExtensions {
    public static IServiceCollection AddReadTask(this IServiceCollection services) => services
        .AddScoped<IScope<Request, Response>, ReadTask>()
        .AddScoped<IReadSocket, ReadSocket>()
        .AddScoped<IReadPlugin, ReadPlugin>();
}
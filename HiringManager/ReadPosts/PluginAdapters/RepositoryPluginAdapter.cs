
namespace Blogger.ReadPosts.PluginAdapters;

public class RepositoryPluginAdapter(IRepositoryPlugin repositoryPlugin) : Business.IRepositoryPluginAdapter
{
    public async Task<List<Core.Business.Post>> Read(Business.Request request, CancellationToken token)
    {
        var adapter = await repositoryPlugin.Read(request.Title, request.Content, token);
        var business = adapter.Select(x => new Core.Business.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return business;
    }
}

public interface IRepositoryPlugin
{
    Task<List<Core.PluginAdapters.Post>> Read(string title, string content, CancellationToken token);
}


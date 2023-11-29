
namespace Blogger.ReadPosts.PluginAdapters;

public class DataAccessAdapter(IDataAccessPlugin dataAccessPlugin) : Business.IDataAccessAdapter
{
    public async Task<List<Core.Business.Post>> Read(Business.Request request, CancellationToken token)
    {
        var adapter = await dataAccessPlugin.Read(request.Title, request.Content, token);
        var business = adapter.Select(x => new Core.Business.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return business;
    }
}

public interface IDataAccessPlugin
{
    Task<List<Core.PluginAdapters.Post>> Read(string title, string content, CancellationToken token);
}


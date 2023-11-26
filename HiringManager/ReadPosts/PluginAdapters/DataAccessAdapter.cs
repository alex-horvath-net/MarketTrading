
namespace Blogger.ReadPosts.PluginAdapters;

public class DataAccessAdapter(IDataAccess dataAccess) : Business.IDataAccessAdapter
{
    public async Task<List<Core.Business.Post>> Read(Business.Request request, CancellationToken token)
    {
        var adapter = await dataAccess.Read(request.Title, request.Content, token);
        var business = adapter.Select(x => new Core.Business.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return business;
    }
}

public interface IDataAccess
{
    Task<List<Core.PluginAdapters.Post>> Read(string title, string content, CancellationToken token);
}


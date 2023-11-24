using Blogger.ReadPosts.Business;
using Core.Business.DomainModel;

namespace Blogger.ReadPosts.PluginAdapters;

public class RepositoryPluginAdapter(PluginAdapters.IRepositoryPlugin repositoryPlugin) : IRepositoryPluginAdapter
{
    public async Task<List<Post>> Read(Request request, CancellationToken token)
    {
        var adapter = await repositoryPlugin.Read(request.Title, request.Content, token);
        var business = adapter.Select(x => new Core.Business.DomainModel.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return business;
    }
}


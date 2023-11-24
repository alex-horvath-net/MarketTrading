using Core.PluginAdapters.DataModel;

namespace Blogger.ReadPosts.PluginAdapters;

public interface IRepositoryPlugin
{
    Task<List<Post>> Read(string title, string content, CancellationToken token);
}


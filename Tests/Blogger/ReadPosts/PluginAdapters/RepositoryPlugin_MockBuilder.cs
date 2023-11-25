using Blogger.ReadPosts.PluginAdapters;
using Core.PluginAdapters.DataModel;

namespace Tests.Blogger.ReadPosts.PluginAdapters;

public class RepositoryPlugin_MockBuilder
{
    public readonly IRepositoryPlugin Mock = Substitute.For<IRepositoryPlugin>();
    public List<Post> Results { get; internal set; }

    public RepositoryPlugin_MockBuilder() => MockRead();

    public RepositoryPlugin_MockBuilder MockRead()
    {
        Results = new List<Post>
        {
            new Post() {  Title= "Title", Content="Content"}
        };
        Mock.Read(default, default, default).ReturnsForAnyArgs(Results);
        return this;
    }
}

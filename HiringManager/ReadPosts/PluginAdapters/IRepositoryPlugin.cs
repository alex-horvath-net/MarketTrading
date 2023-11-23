using Core.PluginAdapters.DataModel;
using NSubstitute;

namespace Blogger.ReadPosts.PluginAdapters;

public interface IRepositoryPlugin
{
    Task<List<Post>> Read(string title, string content, CancellationToken token);

    public class MockBuilder
    {
        public readonly PluginAdapters.IRepositoryPlugin Mock = Substitute.For<PluginAdapters.IRepositoryPlugin>();
        public List<Post> Results { get; internal set; }

        public MockBuilder() => MockRead();

        public MockBuilder MockRead()
        {
            Results = new List<Post>
            {
                new Core.PluginAdapters.DataModel.Post() {  Title= "Title", Content="Content"}
            };
            Mock.Read(default, default,default).ReturnsForAnyArgs(Results);
            return this;
        }
    }
}


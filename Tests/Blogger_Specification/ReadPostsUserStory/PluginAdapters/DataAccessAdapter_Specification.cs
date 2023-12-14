using Blogger.ReadPosts.Tasks.ReadTask.DataAccessSocket;
using Common.Sockets.DataAccess;
using Spec.Blogger_Specification.ReadPostsUserStory.BusinessWorkFlow;

namespace Spec.Blogger_Specification.ReadPostsUserStory.PluginAdapters;

public class DataAccessAdapter_Specification
{
    //[Fact]
    public async void Path_Without_Diversion()
    {
        var unit = new DataAccessSocket(repositoryPlugin.Mock);
        var response = await unit.Read(feature.Request, feature.Token);
        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => repositoryPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
        await repositoryPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
    }

    private readonly RepositoryPlugin_MockBuilder repositoryPlugin = new();
    private readonly Featrue_MockBuilder feature = new();
}

public class RepositoryPlugin_MockBuilder
{
    public readonly IDataAccessPlugin Mock = Substitute.For<IDataAccessPlugin>();
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

using Blogger.ReadPosts.PluginAdapters;
using FluentAssertions;
using NSubstitute;
using Tests.Blogger.ReadPosts.Business;

namespace Tests.Blogger.ReadPosts.PluginAdapters;

public class RepositoryPluginAdapter_Specification
{
    [Fact]
    public async void Path_Without_Diversion()
    {
        var unit = new RepositoryPluginAdapter(repositoryPlugin.Mock);
        var response = await unit.Read(feature.Request, feature.Token);
        response.Should().NotBeNullOrEmpty();
        response.Should().OnlyContain(result => repositoryPlugin.Results.Any(x => x.Title == result.Title && x.Content == result.Content));
        await repositoryPlugin.Mock.ReceivedWithAnyArgs(1).Read(default, default, default);
    }

    private readonly RepositoryPlugin_MockBuilder repositoryPlugin = new();
    private readonly WorkFlow_MockBuilder feature = new();

}

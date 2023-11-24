using Blogger.ReadPosts.Business;
using FluentAssertions;
using NSubstitute;

namespace Tests.Blogger.ReadPosts.Business;

public class WorkFlow_Specification
{
    [Fact]
    public async void Invalid_Request()
    {
        validatorAdapter.MockFailedValidation();

        var unit = new WorkFlow(validatorAdapter.Mock, repositoryAdapter.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.ValidationResults.Should().NotBeNull();
        await validatorAdapter.Mock.Received(1).Validate(feature.Request, feature.Token);
        response.Posts.Should().BeNull();
        await repositoryAdapter.Mock.Received(0).Read(feature.Request, feature.Token);
    }

    [Fact]
    public async void Valid_Request()
    {
        var unit = new WorkFlow(validatorAdapter.Mock, repositoryAdapter.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.ValidationResults.Should().NotBeNull();
        await validatorAdapter.Mock.Received(1).Validate(feature.Request, feature.Token);
        response.Posts.Should().NotBeNull();
        response.Posts.Should().OnlyContain(post => post.Title.Contains(feature.Request.Title) || post.Content.Contains(feature.Request.Content));
        await repositoryAdapter.Mock.Received(1).Read(feature.Request, feature.Token);
    }

    private readonly ValidatorPluginAdapter_MockBuilder validatorAdapter = new();
    private readonly RepositoryPluginAdapter_MockBuilder repositoryAdapter = new();
    private readonly WorkFlow_MockBuilder feature = new();
}

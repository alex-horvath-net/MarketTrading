using Blogger.ReadPosts.Business;
using Core.Business;

namespace Specifications.Blogger_Specification.ReadPosts.Business;
                                                         
public class Feature_Specification
{
    [Fact]
    public async void Invalid_Request()
    {
        validatorAdapter.MockFailedValidation();

        var unit = new Feature(validatorAdapter.Mock, repositoryAdapter.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.RequestIssues.Should().NotBeNull();
        await validatorAdapter.Mock.Received(1).Validate(feature.Request, feature.Token);
        response.Posts.Should().BeNull();
        await repositoryAdapter.Mock.Received(0).Read(feature.Request, feature.Token);
    }

    [Fact]
    public async void Valid_Request()
    {
        var unit = new Feature(validatorAdapter.Mock, repositoryAdapter.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.RequestIssues.Should().NotBeNull();
        await validatorAdapter.Mock.Received(1).Validate(feature.Request, feature.Token);
        response.Posts.Should().NotBeNull();
        response.Posts.Should().OnlyContain(post => post.Title.Contains(feature.Request.Title) || post.Content.Contains(feature.Request.Content));
        await repositoryAdapter.Mock.Received(1).Read(feature.Request, feature.Token);
    }

    private readonly ValidatorPluginAdapter_MockBuilder validatorAdapter = new();
    private readonly RepositoryPluginAdapter_MockBuilder repositoryAdapter = new();
    private readonly Featrue_MockBuilder feature = new();
}

public class Featrue_MockBuilder
{
    public readonly IFeature Mock = Substitute.For<IFeature>();
    public Request Request;
    public CancellationToken Token;

    public Featrue_MockBuilder() => UseValidRequest().UseNoneCanceledToken();

    public Featrue_MockBuilder UseValidRequest()
    {
        Request = new Request("Title", "Content");
        Request = Request with { Title = Request.Title, Content = Request.Content };
        return this;
    }

    public Featrue_MockBuilder UseInvalidRequest()
    {
        Request = new Request(null, null);
        Request = Request with { Title = Request.Title, Content = Request.Content };
        return this;
    }

    public Featrue_MockBuilder UseNoneCanceledToken()
    {
        Token = CancellationToken.None;
        return this;
    }
}

public class RepositoryPluginAdapter_MockBuilder
{
    public readonly IDataAccessAdapter Mock = Substitute.For<IDataAccessAdapter>();

    public RepositoryPluginAdapter_MockBuilder() => MockReadPosts();

    public RepositoryPluginAdapter_MockBuilder MockReadPosts()
    {
        var response = new List<Post>
            {
                new Post{ Title= "Title1", Content= "Content1"},
                new Post{ Title= "Title2", Content= "Content2"},
                new Post{ Title= "Title3", Content= "Content3"}
            };
        Mock.Read(default, default).ReturnsForAnyArgs(response);
        return this;
    }

}

public class ValidatorPluginAdapter_MockBuilder
{
    public readonly Blogger.ReadPosts.Business.IValidationAdapter Mock = Substitute.For<IValidationAdapter>();

    public ValidatorPluginAdapter_MockBuilder() => MockPassedValidation();

    public ValidatorPluginAdapter_MockBuilder MockPassedValidation()
    {
        var result = new List<ValidationResult> { ValidationResult.Success() };
        Mock.Validate(default, default).ReturnsForAnyArgs(result);
        return this;
    }

    public ValidatorPluginAdapter_MockBuilder MockFailedValidation()
    {
        var result = new List<ValidationResult> { ValidationResult.Failed("errorCode1", "errorMessage1") };
        Mock.Validate(default, default).ReturnsForAnyArgs(result);
        return this;
    }
}
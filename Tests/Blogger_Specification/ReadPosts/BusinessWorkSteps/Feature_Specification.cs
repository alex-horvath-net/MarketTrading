//using Blogger.ReadPosts.BusinessWorkFlow;
//using Blogger.ReadPosts.PluginAdapters;
//using Core.BusinessWorkFlow;

//namespace Specifications.Blogger_Specification.ReadPosts.BusinessWorkFlow;

//public class UserStory_Spec
//{
//    //[Fact]
//    public async void NonStoppedFeature()
//    {
//        workSteps.MockFailedValidation();

//        //var unit = new UserStory(workSteps.Mock, repositoryAdapter.Mock);
//        var unit = new UserStory(default, default);
//        var response = await unit.Run(feature.Request, feature.Token);

//        response.Should().NotBeNull();
//        response.Request.Should().Be(feature.Request);
//        response.Validations.Should().NotBeNull();
//        await workSteps.Mock.Received(1).Validate(feature.Request, feature.Token);
//        response.Posts.Should().BeNull();
//        await repositoryAdapter.Mock.Received(0).Read(feature.Request, feature.Token);
//    }

//    //[Fact]
//    public async void Valid_Request()
//    {
//        //var unit = new UserStory(workSteps.Mock, repositoryAdapter.Mock);
//        var unit = new UserStory(default, default);
//        var response = await unit.Run(feature.Request, feature.Token);

//        response.Should().NotBeNull();
//        response.Request.Should().Be(feature.Request);
//        response.Validations.Should().NotBeNull();
//        await workSteps.Mock.Received(1).Validate(feature.Request, feature.Token);
//        response.Posts.Should().NotBeNull();
//        response.Posts.Should().OnlyContain(post => post.Title.Contains(feature.Request.Title) || post.Content.Contains(feature.Request.Content));
//        await repositoryAdapter.Mock.Received(1).Read(feature.Request, feature.Token);
//    }

//    private readonly Tasks_MockBuilder workSteps = new();
//    private readonly RepositoryPluginAdapter_MockBuilder repositoryAdapter = new();
//    private readonly Featrue_MockBuilder feature = new();
//}

//public class Featrue_MockBuilder
//{
//    public readonly IFeature<Request, Response> Mock = Substitute.For<IFeature<Request,Response>>();
//    public Request Request;
//    public CancellationToken Token;

//    public Featrue_MockBuilder() => UseValidRequest().UseNoneCanceledToken();

//    public Featrue_MockBuilder UseValidRequest()
//    {
//        Request = new Request("Title", "Content");
//        Request = Request with { Title = Request.Title, Content = Request.Content };
//        return this;
//    }

//    public Featrue_MockBuilder UseInvalidRequest()
//    {
//        Request = new Request(null, null);
//        Request = Request with { Title = Request.Title, Content = Request.Content };
//        return this;
//    }

//    public Featrue_MockBuilder UseNoneCanceledToken()
//    {
//        Token = CancellationToken.None;
//        return this;
//    }
//}

//public class RepositoryPluginAdapter_MockBuilder
//{
//    public readonly IDataAccess Mock = Substitute.For<IDataAccess>();

//    public RepositoryPluginAdapter_MockBuilder() => MockReadPosts();

//    public RepositoryPluginAdapter_MockBuilder MockReadPosts()
//    {
//        var response = new List<Post>
//            {
//                new Post{ Title= "Title1", Content= "Content1"},
//                new Post{ Title= "Title2", Content= "Content2"},
//                new Post{ Title= "Title3", Content= "Content3"}
//            };
//        Mock.Read(default, default).ReturnsForAnyArgs(response);
//        return this;
//    }

//}

//public class Tasks_MockBuilder
//{
//    public readonly IValidationAdapter Mock = Substitute.For<IValidationAdapter>();

//    public Tasks_MockBuilder() => MockNotStoppedWorkSteps();

//    public Tasks_MockBuilder MockNotStoppedWorkSteps()
//    {
//        var result = new List<ValidationResult> { ValidationResult.Success() };
//        Mock.Validate(default, default).ReturnsForAnyArgs(result);
//        return this;
//    }

//    public Tasks_MockBuilder MockFailedValidation()
//    {
//        var result = new List<ValidationResult> { ValidationResult.Failed("errorCode1", "errorMessage1") };
//        Mock.Validate(default, default).ReturnsForAnyArgs(result);
//        return this;
//    }
//}
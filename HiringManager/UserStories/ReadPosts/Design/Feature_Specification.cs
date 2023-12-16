using Blogger.UserStories.ReadPosts;
using Core.UserStory;

namespace Blogger.UserStories.ReadPosts.Design;

public class Feature_Specification
{
    //[Fact]
    public async void NonStoppedFeature()
    {
        workSteps.UseNonStoppedWorkSteps();

        var unit = new UserStoryCore<Request, Response>(workSteps.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.CanRun.Should().BeFalse();
        response.Posts.Should().BeNull();
        response.Validations.Should().BeNull();
    }

    //[Fact]
    public async void StoppedFeature()
    {
        workSteps.MockStoppedWorkSteps();

        var unit = new UserStoryCore<Request, Response>(workSteps.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.CanRun.Should().BeTrue();
    }

    private readonly WorkStep_MockBuilder workSteps = new();
    private readonly Featrue_MockBuilder feature = new();
}

public class Featrue_MockBuilder
{
    public readonly IUserStory<Request, Response> Mock = Substitute.For<IUserStory<Request, Response>>();
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

public class WorkStep_MockBuilder
{
    public readonly List<ITask<Request, Response>> Mock = new List<ITask<Request, Response>>();

    public WorkStep_MockBuilder() => UseNonStoppedWorkSteps();

    public WorkStep_MockBuilder UseNonStoppedWorkSteps()
    {
        Mock.Clear();
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new ContinueWorkStep());
        return this;
    }

    public WorkStep_MockBuilder MockStoppedWorkSteps()
    {
        Mock.Clear();
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new StopWorkStep());
        Mock.Add(new ContinueWorkStep());
        return this;
    }

    public class StopWorkStep : ITask<Request, Response>
    {
        public Task Run(Response response, CancellationToken token)
        {
            response.CanRun = true;
            return Task.CompletedTask;
        }
    }

    public class ContinueWorkStep : ITask<Request, Response>
    {
        public Task Run(Response response, CancellationToken token)
        {
            response.CanRun = false;
            return Task.CompletedTask;
        }
    }
}
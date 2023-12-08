namespace Sys.UserStory;

public abstract class UserStoryCore<TRequest, TResponse>(IEnumerable<ITask<TResponse>> workSteps)
         : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()

{
    public async Task<TResponse> Run(TRequest request, CancellationToken cancellation)
    {
        var response = new TResponse() with { Request = request };
        foreach (var workStep in workSteps)
        {
            await workStep.Run(response, cancellation);
            cancellation.ThrowIfCancellationRequested();
            if (response.Stopped) return response;
        }
        return response;
    }
}

public record RequestCore();

public record ResponseCore<TRequest>() where TRequest : RequestCore
{
    public TRequest Request { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
    public bool Stopped { get; set; }
}

//--Specification--------------------------------------------------


public class UserStory_Spec
{
    [Fact]
    public async void NonStoppedFeature()
    {
        workSteps.UseNonStoppedWorkSteps();

        var unit = new UserStory(workSteps.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.Stopped.Should().BeFalse();
        response.Posts.Should().BeNull();
        response.Validations.Should().BeNull();
    }

    [Fact]
    public async void StoppedFeature()
    {
        workSteps.MockStoppedWorkSteps();

        var unit = new UserStory(workSteps.Mock);
        var response = await unit.Run(feature.Request, feature.Token);

        response.Should().NotBeNull();
        response.Request.Should().Be(feature.Request);
        response.Stopped.Should().BeTrue();
    }

    private readonly ITask.MockBuilder workSteps = new();
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

public class Tasks_MockBuilder
{
    public readonly List<ITask<Response>> Mock = new List<ITask<Response>>();

    public Tasks_MockBuilder() => UseNonStoppedWorkSteps();

    public Tasks_MockBuilder UseNonStoppedWorkSteps()
    {
        Mock.Clear();
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new ContinueWorkStep());
        return this;
    }

    public Tasks_MockBuilder MockStoppedWorkSteps()
    {
        Mock.Clear();
        Mock.Add(new ContinueWorkStep());
        Mock.Add(new StopWorkStep());
        Mock.Add(new ContinueWorkStep());
        return this;
    }

    public class StopWorkStep : ITask<Response>
    {
        public Task Run(Response response, CancellationToken cancellation)
        {
            response.Stopped = true;
            return Task.CompletedTask;
        }
    }

    public class ContinueWorkStep : ITask<Response>
    {
        public Task Run(Response response, CancellationToken cancellation)
        {
            response.Stopped = false;
            return Task.CompletedTask;
        }
    }
}
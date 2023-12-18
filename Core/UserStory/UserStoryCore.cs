using FluentAssertions;
using Xunit;

namespace Core.UserStory;

public class UserStoryCore<TRequest, TResponse>(IEnumerable<ITask<TRequest, TResponse>> tasks) : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    public async Task<TResponse> Run(TRequest request, CancellationToken token)
    {
        var response = new TResponse() with { Request = request };
        foreach (var task in tasks)
        {
            if (response.CanRun)
                await task.Run(response, token);
            else
                break;
            token.ThrowIfCancellationRequested();
        }
        return response;
    }
}

public class UserStoryCore_Design
{
    [Fact]
    public async void NonStoppedFeature()
    {
        var tasks = new List<ITask<RequestCore, ResponseCore<RequestCore>>>()
        {
            oneTask.DoNotTerminate().Mock,
            otherTask.DoNotTerminate().Mock
        }.AsEnumerable();
        var request = new RequestCore();
        var token = CancellationToken.None;
        var unit = new UserStoryCore<RequestCore, ResponseCore<RequestCore>>(tasks);

        var response = await unit.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.CanRun.Should().BeTrue();
        response.Validations.Should().BeNull();
    }

    [Fact]
    public async void StoppedFeature()
    {
        var tasks = new List<ITask<RequestCore, ResponseCore<RequestCore>>>()
        {
            oneTask.DoNotTerminate().Mock,
            otherTask.Terminate().Mock
        }.AsEnumerable();

        var request = new RequestCore();
        var token = CancellationToken.None;
        var unit = new UserStoryCore<RequestCore, ResponseCore<RequestCore>>(tasks);
        var response = await unit.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.CanRun.Should().BeFalse();
    }

    private readonly ITask<RequestCore,ResponseCore<RequestCore>>.MockBuilder otherTask = new();
    private readonly ITask<RequestCore, ResponseCore<RequestCore>>.MockBuilder oneTask = new();
    //private readonly Featrue_MockBuilder feature = new();
}


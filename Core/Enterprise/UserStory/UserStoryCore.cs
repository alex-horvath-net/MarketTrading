using FluentAssertions;
using Xunit;

namespace Core.Enterprise.UserStory;

public class UserStoryCore<TRequest, TResponse>(IEnumerable<IUserTask<TRequest, TResponse>> userTasks) : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    public async Task<TResponse> Run(TRequest request, CancellationToken token)
    {
        var response = new TResponse() with { Request = request };
        foreach (var userTask in userTasks)
        {
            var terminated = await userTask.Run(response, token);
            if (terminated)
                break;
        }
        return response;
    }
}

public class UserStoryCore_Design
{
    [Fact]
    public async void NonStoppedFeature()
    {
        var tasks = new[]
        {
            oneTask.DoNotTerminate().Mock,
            otherTask.DoNotTerminate().Mock
        };
        var request = new RequestCore();
        var token = CancellationToken.None;
        var unit = new UserStoryCore<RequestCore, ResponseCore<RequestCore>>(tasks);

        var response = await unit.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.Validations.Should().BeNull();
    }

    [Fact]
    public async void StoppedFeature()
    {
        var tasks = new[]
        {
            oneTask.Terminate().Mock,
            otherTask.DoNotTerminate().Mock
        };
        var request = new RequestCore();
        var token = CancellationToken.None;
        var unit = new UserStoryCore<RequestCore, ResponseCore<RequestCore>>(tasks);

        var response = await unit.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        //oneTask.Mock.Received(1).Run(response, token);  
    }

    private readonly IUserTask<RequestCore, ResponseCore<RequestCore>>.MockBuilder otherTask = new();
    private readonly IUserTask<RequestCore, ResponseCore<RequestCore>>.MockBuilder oneTask = new();
    //private readonly Featrue_MockBuilder feature = new();
}


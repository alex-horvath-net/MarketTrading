using FluentAssertions;
using Xunit;

namespace Core.Enterprise.UserStory;

public class UserStoryCore<TRequest, TResponse>(IEnumerable<IUserTask<TRequest, TResponse>> userTasks) : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    public async Task<TResponse> Run(TRequest request, CancellationToken token)
    {
        var response = new TResponse() { Request = request };
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
        var userStory = new UserStoryCore<RequestCore, ResponseCore<RequestCore>>(
            [
                oneTask.DoNotTerminate().Mock,
                otherTask.DoNotTerminate().Mock
            ]);

        var response = await userStory.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.Validations.Should().BeNull();
    }

    [Fact]
    public async void StoppedFeature()
    {
        var userStory = new UserStoryCore<RequestCore, ResponseCore<RequestCore>>(
            [
                oneTask.Terminate().Mock,
                otherTask.DoNotTerminate().Mock
            ]);

        var response = await userStory.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);

    }

    private readonly IUserTask<RequestCore, ResponseCore<RequestCore>>.MockBuilder otherTask = new();
    private readonly IUserTask<RequestCore, ResponseCore<RequestCore>>.MockBuilder oneTask = new();
    private readonly RequestCore request = new();
    private readonly CancellationToken token = CancellationToken.None;
}


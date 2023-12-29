using Core.Enterprise.Plugins.FP;
using Core.Enterprise.UserStory;
using FluentAssertions;
using Xunit;

namespace Core.Enterprise.UserTasks;

public class FeatureTask<TRequest, TResponse> : IUserTask<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
{
    public Task<bool> Run(TResponse response, CancellationToken token)
    {
        response.FeatureEnabled = false;
        return (!response.FeatureEnabled).ToTask();
    }
}

public class FeatureTask_Design
{
    [Fact]
    public async void FeatureFlagIsFalse()
    {
        var response = new ResponseCore<RequestCore>();
        var token = CancellationToken.None;
        var unit = new FeatureTask<RequestCore, ResponseCore<RequestCore>>();

        var terminated = await unit.Run(response, token);

        response.FeatureEnabled.Should().BeFalse();
        terminated.Should().BeTrue();
    }
}
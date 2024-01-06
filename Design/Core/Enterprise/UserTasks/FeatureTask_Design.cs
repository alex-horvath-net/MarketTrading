using Core.Enterprise.UserStory;
using Core.Enterprise.UserTasks;

namespace Design.Core.Enterprise.UserTasks;

public class FeatureTask_Design
{
    [Fact]
    public async void FeatureFlagIsFalse()
    {
        var response = new Response<Request>();
        var token = CancellationToken.None;
        var unit = new FeatureTask<Request, Response<Request>>();

        await unit.Run(response, token);

        response.FeatureEnabled.Should().BeFalse();
        response.Terminated.Should().BeTrue();
    }
}
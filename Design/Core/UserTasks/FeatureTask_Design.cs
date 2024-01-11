using Core.UserTasks;
using Core.UserStory.DomainModel;

namespace Design.Core.UserTasks;

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
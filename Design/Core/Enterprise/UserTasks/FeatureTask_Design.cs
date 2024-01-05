using Core.Enterprise.UserTasks;

namespace Design.Core.Enterprise.UserTasks;

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
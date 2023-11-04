using HiringManager.ReadJobRoles.Feature;

namespace Design.As_A_HiringManager.I_Want_To_Read_JobRoles;

public class FeatureDesign
{
    [Fact]
    public async void Can_Be_Called()
    {
        var request = new Request();
        var token = CancellationToken.None;
        var feature = new Feature();
        var response = await feature.Run(request, token);
        Assert.NotNull(response);
    }
}
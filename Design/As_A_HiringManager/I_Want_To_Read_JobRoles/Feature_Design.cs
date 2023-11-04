using HiringManager.ReadJobRoles.Feature;

namespace Design.As_A_HiringManager.I_Want_To_Read_JobRoles;

public class Feature_Design
{
    [Fact]
    public async Task Can_Be_Called()
    {
        var request = new Request();
        var token = CancellationToken.None;
        var feature = new Feature();
        var response = await feature.Run(request, token);
        response.ShouldBe_NotNull();
    }

    [Fact]
    public async Task Can_Read_JobRoles()
    {
        var request = new Request();
        var token = CancellationToken.None;
        var feature = new Feature();
        var response = await feature.Run(request, token);
        response.JobRoles.ShouldBe_NotEmpty();
    }
}
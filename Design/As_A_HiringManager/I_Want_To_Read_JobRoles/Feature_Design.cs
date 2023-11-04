namespace Design.As_A_HiringManager.I_Want_To_Read_JobRoles;

public class Feature_Design
{
    [Fact]
    public async Task Can_Read_JobRoles()
    {
        var request = context.GetRequest(); 
        var token = context.GetToken();
        var unit = context.GetUnit();
      
        var response = await unit.Run(request,token);
        
        response.JobRoles.ShouldBe_NotEmpty();
        response.JobRoles[0].Name.ShouldBe(request.Name);
    }

    private readonly Feature_Design_Context context = new();
}
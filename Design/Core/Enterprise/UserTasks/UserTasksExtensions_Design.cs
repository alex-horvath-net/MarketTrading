using Core.Enterprise.UserStory;
using Core.Enterprise.UserTasks;
using Microsoft.Extensions.DependencyInjection;

namespace Design.Core.Enterprise.UserTasks;

public class Extensions_Design
{
    [Fact]
    public void AddFeatureTask_Registers_All_UserTask()
    {
        var services = new ServiceCollection();

        services.AddFeatureTask();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserTask<RequestCore, ResponseCore<RequestCore>>>();
        userStory.Should().NotBeNull();
    }
}


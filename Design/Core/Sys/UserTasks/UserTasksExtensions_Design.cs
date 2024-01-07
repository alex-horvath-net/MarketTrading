using Core.Sys.UserStory;
using Core.Sys.UserStory.DomainModel;
using Core.Sys.UserTasks;
using Microsoft.Extensions.DependencyInjection;

namespace Design.Core.Sys.UserTasks;

public class Extensions_Design
{
    [Fact]
    public void AddFeatureTask_Registers_All_UserTask()
    {
        var services = new ServiceCollection();

        services.AddFeatureTask();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserTask<Request, Response<Request>>>();
        userStory.Should().NotBeNull();
    }
}


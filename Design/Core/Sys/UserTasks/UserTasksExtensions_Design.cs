using AppPolicy.UserStory;
using AppPolicy.UserStory.DomainModel;
using AppPolicy.UserTasks;
using Microsoft.Extensions.DependencyInjection;

namespace Design.AppPolicy.UserTasks;

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


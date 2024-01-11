using Core.UserStory;
using Core.UserStory.DomainModel;
using Microsoft.Extensions.DependencyInjection;

namespace Core.UserTasks;

public class Extensions_Design {
    [Fact]
    public void AddFeatureTask_Registers_All_UserTask() {
        var services = new ServiceCollection();

        services.AddFeatureTask();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IScope<Request, Response<Request>>>();
        userStory.Should().NotBeNull();
    }
}


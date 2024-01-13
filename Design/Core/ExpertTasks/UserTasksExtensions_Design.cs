using Core.ExpertStory;
using Core.ExpertStory.DomainModel;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ExpertTasks;

public class Extensions_Design {
    [Fact]
    public void AddFeatureTask_Registers_All_UserTask() {
        var services = new ServiceCollection();

        services.AddFeatureTask();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IExpertTask<Request, Response<Request>>>();
        userStory.Should().NotBeNull();
    }
}


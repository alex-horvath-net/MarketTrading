using Core.ExpertStory;
using Core.ExpertStory.StoryModel;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public class Extensions_Design {
    [Fact]
    public void AddUserStory_Registers_All_UserStory() {
        var services = new ServiceCollection();

        services.AddCoreSystem();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IExpertStory<Request, Response<Request>>>();
        userStory.Should().NotBeNull();
    }
}

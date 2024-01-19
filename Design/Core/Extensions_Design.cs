using Core.Story;
using Core.Story.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public class Extensions_Design {
    [Fact]
    public void AddUserStory_Registers_All_UserStory() {
        var services = new ServiceCollection();

        services.AddCoreSystem();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IStory<Request, Response<Request>>>();
        userStory.Should().NotBeNull();
    }
}

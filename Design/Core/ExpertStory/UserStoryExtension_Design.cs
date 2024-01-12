using Core.ExpertStory.DomainModel;
using Microsoft.Extensions.DependencyInjection;
namespace Core.ExpertStory;

public class UserStoryExtension_Design {
    [Fact]
    public void AddUserStory_Registers_All_UserStory() {
        var services = new ServiceCollection();

        services.AddUserStory();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IExpertStory<Request, Response<Request>>>();
        userStory.Should().NotBeNull();
    }
}


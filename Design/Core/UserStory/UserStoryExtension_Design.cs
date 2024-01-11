using Core.UserStory;
using Core.UserStory.DomainModel;
using Microsoft.Extensions.DependencyInjection;
namespace Design.Core.UserStory;

public class UserStoryExtension_Design
{
    [Fact]
    public void AddUserStory_Registers_All_UserStory()
    {
        var services = new ServiceCollection();

        services.AddUserStory();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserStory<Request, Response<Request>>>();
        userStory.Should().NotBeNull();
    }
}


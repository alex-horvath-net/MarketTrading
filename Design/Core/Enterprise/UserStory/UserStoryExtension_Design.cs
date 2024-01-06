using Core.Enterprise.UserStory;
using Microsoft.Extensions.DependencyInjection;
namespace Design.Core.Enterprise.UserStory;

public class UserStoryExtension_Design
{
    [Fact]
    public void AddUserStory_Registers_All_UserStory()
    {
        var services = new ServiceCollection();

        services.AddUserStory();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserStory<RequestCore, Response<RequestCore>>>();
        userStory.Should().NotBeNull();
    }
}


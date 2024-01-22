using Common.Business;
using Microsoft.Extensions.DependencyInjection;
namespace Common.Problem;

public class UserStoryExtension_Design
{
    [Fact]
    public void AddUserStory_Registers_All_UserStory()
    {
        var services = new ServiceCollection();

        services.AddStory();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IStory<Request, Response<Request>>>();
        userStory.Should().NotBeNull();
    }
}


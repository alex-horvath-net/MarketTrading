using Microsoft.Extensions.DependencyInjection;
namespace Core.Business;

public class Extensions_Design {
    [Fact]
    public void AddUserStory_Registers_All_UserStory() {
        var services = new ServiceCollection();

        services.AddCoreBusiness();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IStory<RequestCore, ResponseCore<RequestCore>, TestStory>>();
        userStory.Should().NotBeNull();
    }
}


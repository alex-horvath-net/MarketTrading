using Core.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public class Extensions_Design {
    [Fact]
    public void AddUserStory_Registers_All_UserStory() {
        var services = new ServiceCollection();

        services.AddStory();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IStory<RequestCore, ResponseCore<RequestCore>>>();
        userStory.Should().NotBeNull();
    }
}

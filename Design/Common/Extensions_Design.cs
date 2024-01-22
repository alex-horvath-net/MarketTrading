using Core;
using Core.Business;
using Core.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public class Extensions_Design {
    [Fact]
    public void AddUserStory_Registers_All_UserStory() {
        var services = new ServiceCollection();

        services.AddCoreSolutions(null);
        services.AddCoreBusiness();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IStory<RequestCore, ResponseCore<RequestCore>, TestStory>>();
        userStory.Should().NotBeNull();
    }
}

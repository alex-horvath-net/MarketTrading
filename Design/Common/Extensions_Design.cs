using Core;
using Core.Business;
using Core.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public class Extensions_Design {
    [Fact]
    public void AddUserStory_Registers_All_UserStory() {
        var services = new ServiceCollection();
        var filePath =
            Path.GetFullPath(
                Path.Combine(
                    Environment.CurrentDirectory,
                    "../../../../Design/appsettings.json"));
        var configuration = new ConfigurationBuilder().AddJsonFile(filePath).Build();
        services.AddCoreSolutions();
        services.AddCoreBusiness();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserStoryCore<RequestCore, ResponseCore<RequestCore>, SettingsCore>>();
        userStory.Should().NotBeNull();
    }
}

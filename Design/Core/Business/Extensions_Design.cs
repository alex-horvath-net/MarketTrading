using System.Reflection;
using Core.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Core.Business;

public class Extensions_Design {
    [Fact]
    public void AddUserStory_Registers_All_UserStory() {
        var services = new ServiceCollection();
        var filePath = 
            Path.GetFullPath(
                Path.Combine(
                    Environment.CurrentDirectory, 
                    "../../../../Design/appsettings.json"));

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(filePath)
            .Build();

        services.AddCore();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserStory<RequestCore, ResponseCore<RequestCore>, SettingsCore>>();
        userStory.Should().NotBeNull();
    }    
}
 

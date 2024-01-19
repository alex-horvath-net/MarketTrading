using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Story.Solutions.Data.MainDB;

namespace Experts.Blogger;

public class Extensions_Design {
    [Fact]
    public void Expert_Is_Available() {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();
        var environment = Environments.Development;
        var services = new ServiceCollection(); 

        services
            .AddMainDB(configuration, environment)
            .AddBlogger();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<Expert>(); 
    }
}

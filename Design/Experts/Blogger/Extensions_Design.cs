using Common.Solutions.Data.MainDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Experts.Blogger;

public class Extensions_Design {
    [Fact]
    public void Expert_Is_Available() {
        var configuration = new ConfigurationBuilder().Build();
        var environment = Environments.Development;
        var services = new ServiceCollection(); 

        services
            .AddMainDB(configuration, environment)
            .AddBlogger();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<Expert>(); 
    }
}

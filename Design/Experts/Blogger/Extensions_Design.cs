using Common;
using Common.Solutions.Data.MainDB;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Experts.Blogger;

public class Extensions_Design {
    [Fact]
    public void Expert_Is_Available() {
        var filePath =
           Path.GetFullPath(
               Path.Combine(
                   Environment.CurrentDirectory,
                   "../../../../Design/appsettings.json"));
        var configuration = new ConfigurationBuilder().AddJsonFile(filePath).Build();
        var services = new ServiceCollection(); 

        services
            .AddCore()
            .AddCommon()
            .AddBlogger();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<Expert>(); 
    }
}

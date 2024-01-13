using Common;
using Core;
using Experts.Blogger;
using Experts.Blogger.ReadPosts.Read;
using Experts.Blogger.ReadPosts.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusinessExperts.Blogger;

public class Extensions_Design {
    [Fact]
    public void AddBlogger_Des() {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();
        var env = Environments.Development;
        var services = new ServiceCollection();

        services
            .AddCoreSystem()
            .AddCoreApplication(configuration, isDevelopment: true)
            .AddBlogger();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<Experts.Blogger.ReadPosts.Read.ISolution>();
        serviceProvider.GetRequiredService<Experts.Blogger.ReadPosts.Validation.ISolution>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

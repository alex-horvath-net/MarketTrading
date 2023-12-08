using App.Plugins;
using Blogger.ReadPosts.Plugins;
using Blogger.ReadPosts.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spec.Blogger_Specification.ReadPosts.Plugins;

public class DependecyInjection_Specification
{
    [Fact]
    public async void Inject_AddReadPosts_Dependecies()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var unit = new ServiceCollection();

        var services = unit
            .AddCore(configuration)
            .AddReadPosts();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<Blogger.ReadPosts.Adapters.IValidation>();
        serviceProvider.GetRequiredService<Blogger.ReadPosts.Adapters.IDataAccess>();

        serviceProvider.GetRequiredService<IValidation>();
        serviceProvider.GetRequiredService<IDataAccess>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

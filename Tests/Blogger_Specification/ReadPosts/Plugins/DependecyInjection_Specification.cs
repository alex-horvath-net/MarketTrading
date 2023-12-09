using BloggerUserRole.ReadPostsFaeture.AdaptersLayer;
using BloggerUserRole.ReadPostsFaeture.PluginsLayer;
using BloggerUserRole.ReadPostsFaeture.TasksLayer;
using BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polices.PluginsLayer;

namespace Spec.Blogger_Specification.ReadPosts.Plugins;

public class DependecyInjection_Specification
{
    //[Fact]
    public async void Inject_AddReadPosts_Dependecies()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var unit = new ServiceCollection();

        var services = unit
            .AddCore(configuration)
            .AddReadPosts();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<IValidation>();
        serviceProvider.GetRequiredService<BloggerUserRole.ReadPostsFaeture.AdaptersLayer.IDataAccess>();

        serviceProvider.GetRequiredService<IValidationAdapter>();
        serviceProvider.GetRequiredService<IDataAccess>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

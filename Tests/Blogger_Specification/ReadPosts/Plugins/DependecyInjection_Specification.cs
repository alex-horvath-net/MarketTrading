using BloggerUserRole.ReadPostsFaeture.AdaptersLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.AdaptersLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.PluginsLayer;
using BloggerUserRole.ReadPostsFaeture.TasksLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;
using Common.PluginsLayer.DataAccessUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        serviceProvider.GetRequiredService<IValidationPlugin>();
        serviceProvider.GetRequiredService<BloggerUserRole.ReadPostsFaeture.AdaptersLayer.DataAccessUnit.IDataAccessPlugin>();

        serviceProvider.GetRequiredService<IValidationAdapter>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

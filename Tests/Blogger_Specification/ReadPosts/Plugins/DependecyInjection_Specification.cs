using BloggerUserRole.ReadPostsFaeture.AdapterLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.AdapterLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.PluginLayer;
using BloggerUserRole.ReadPostsFaeture.TaskLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.TaskLayer.DataAccessUnit;
using Common.PluginLayer.DataAccessUnit;
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
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        serviceProvider.GetRequiredService<IValidationAdapter>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

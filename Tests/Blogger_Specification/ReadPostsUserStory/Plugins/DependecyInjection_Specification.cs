using BloggerUserRole.ReadPostsUserStory;
using BloggerUserRole.ReadPostsUserStory.ReadTask.DataAccessSocket;
using BloggerUserRole.ReadPostsUserStory.ValidationTask;
using BloggerUserRole.ReadPostsUserStory.ValidationTask.ValidationSocket;
using Common.Plugins.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Spec.Blogger_Specification.ReadPostsUserStory.Plugins;

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

        serviceProvider.GetRequiredService<IValidationSocket>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

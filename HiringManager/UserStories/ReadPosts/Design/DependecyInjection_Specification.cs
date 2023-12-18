using Blogger.UserStories.ReadPosts.Tasks.ReadTask.Sockets.DataAccessSocket;
using Blogger.UserStories.ReadPosts.Tasks.ValidationTask;
using Blogger.UserStories.ReadPosts.Tasks.ValidationTask.Sockets.ValidationSocket;
using Common;
using Core;
using Core.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.UserStories.ReadPosts.Design;

public class DependecyInjection_Specification
{
    //[Fact]
    public async void Inject_AddReadPosts_Dependecies()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var unit = new ServiceCollection();

        var services = unit
            .AddCore()
            .AddCommon(configuration)
            .AddBlogger();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<IValidationPlugin>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        serviceProvider.GetRequiredService<IValidationSocket>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

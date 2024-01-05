using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory;
using Core.Enterprise;
using Core.Application;
using Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;
using static Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationTask;
using static Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationSocket;


namespace Design.Users.Blogger.ReadPostsUserStory;

public class Extensions_Design
{
    [Fact]
    public async Task AddReadPostsUserStory()
    {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var services = new ServiceCollection();

        services
            .AddCore()
            .AddCommon(configuration)
            .AddReadPostsUserStory();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<IValidationPlugin>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        serviceProvider.GetRequiredService<IValidationSocket>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory;


namespace Design.Users.Blogger.ReadPostsUserStory;

public static class Extensions_Design
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

        serviceProvider.GetRequiredService<ValidationTask.ValidationSocket.ValidationSocket.IValidationPlugin>();
        serviceProvider.GetRequiredService<Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket.DataAccessPlugin.PluginDesign.IDataAccessPlugin>();

        serviceProvider.GetRequiredService<ValidationTask.ValidationTask.IValidationSocket>();
        serviceProvider.GetRequiredService<IDataAccessPlugin>();

        //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}
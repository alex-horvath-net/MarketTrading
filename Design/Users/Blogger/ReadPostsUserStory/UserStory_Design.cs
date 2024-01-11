using AppCore;
using AppPolicy;
using Experts.Blogger.ReadPostsUserStory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            .AddCoreSystem()
            .AddCoreApplication(configuration)
            .AddReadPostsUserStory();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<IValidationPlugin>();
        serviceProvider.GetRequiredService<IReadPlugin>();

        serviceProvider.GetRequiredService<IValidationSocket>();
        serviceProvider.GetRequiredService<IReadPlugin>();

        //serviceProvider.GetRequiredService<AppPolicy.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<AppPolicy.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}

public class Request_MockBuilder
{
    public Request Mock { get; private set; }

    public Request_MockBuilder UseValidRequest()
    {
        Mock = new Request("Title", "Content");
        return this;
    }

    public Request_MockBuilder UseInvaliedRequestWithMissingFilters()
    {
        Mock = new Request(null, null);
        return this;
    }

    public Request_MockBuilder UseInvaliedRequestWithShortFilters()
    {
        Mock = new Request("12", "21");
        return this;
    }
}

public record Response_MockBuilder
{
    public Response Mock { get; private set; } = new();

    public Response_MockBuilder HasNoPosts()
    {
        WillHaveValidRequest();
        Mock.Posts = null;
        return this;
    }

    public Response_MockBuilder WillHaveValidRequest()
    {
        Mock.Request = new Request_MockBuilder().UseValidRequest().Mock;
        Mock.FeatureEnabled = true;
        Mock.Validations = null;
        return this;
    }

    public Response_MockBuilder HasNoValidations()
    {
        WillHaveValidRequest();
        Mock.Validations = null;
        return this;
    }
}


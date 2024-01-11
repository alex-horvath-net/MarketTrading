using Common;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessExperts.Blogger.ReadPostsExpertStory;

public class Extensions_Design {
    [Fact]
    public async Task AddReadPostsUserStory() {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var services = new ServiceCollection();

        services
            .AddCoreSystem()
            .AddCoreApplication(configuration)
            .AddReadPostsUserStory();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<ValidationTask.ISolution>();
        serviceProvider.GetRequiredService<ReadTask.ISolution>();

        //serviceProvider.GetRequiredService<ValidationTask.ISolutionExpert>();
        //serviceProvider.GetRequiredService<ReadTask.ISolution>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}


public class RequestMockBuilder {
    public Request Mock { get; private set; }

    public RequestMockBuilder UseValidRequest() {
        Mock = new Request("Title", "Content");
        return this;
    }

    public RequestMockBuilder UseInvaliedRequestWithMissingFilters() {
        Mock = new Request(null, null);
        return this;
    }

    public RequestMockBuilder UseInvaliedRequestWithShortFilters() {
        Mock = new Request("12", "21");
        return this;
    }
}


public record ResponseMockBuilder {
    public Response Mock { get; private set; } = new();

    public ResponseMockBuilder HasNoPosts() {
        WillHaveValidRequest();
        Mock.Posts = null;
        return this;
    }

    public ResponseMockBuilder WillHaveValidRequest() {
        Mock.Request = new RequestMockBuilder().UseValidRequest().Mock;
        Mock.FeatureEnabled = true;
        Mock.Validations = null;
        return this;
    }

    public ResponseMockBuilder HasNoValidations() {
        WillHaveValidRequest();
        Mock.Validations = null;
        return this;
    }
}


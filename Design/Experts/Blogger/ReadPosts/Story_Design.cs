using Common;
using Core;
using Experts.Blogger.ReadPosts.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public class Extensions_Design {
    [Fact]
    public async Task AddBlogger_Test() {
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var services = new ServiceCollection();

        services
            .AddCoreSystem()
            .AddCoreApplication(configuration)
            .AddBlogger();

        using var serviceProvider = services.BuildServiceProvider();

        serviceProvider.GetRequiredService<Validation.ISolution>();
        serviceProvider.GetRequiredService<Read.ISolution>();

        //serviceProvider.GetRequiredService<ValidationTask.ISolutionExpert>();
        //serviceProvider.GetRequiredService<ReadTask.ISolution>();

        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
        //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
    }
}


public static class RequestExtensions {
    public static Request MockValidRequest(this Request request) =>
        new Request("Title", "Content");

    public static Request MockMissingpProperties(this Request request) =>
        request = new Request(null, null);

    public static Request MockTooShortProperties(this Request request) =>
        new Request("12", "21");
}

public static class ResponseExtensions {
    public static Response MockNoPosts(this Response response) {
        response.MockValidRequest();
        response.Posts = null;
        return response;
    }

    public static Response MockValidRequest(this Response response) {
        response.Request = Request.Empty().MockValidRequest();
        response.FeatureEnabled = true;
        response.Validations = null;
        return response;
    }

    public static Response MockNoValidations(this Response response) {
        response.MockValidRequest();
        response.Validations = null;
        return response;
    }
}


using Azure;
using Common.Business.Model;
using Core.Business;
using Core.Business.Model;
using Core.Solutions.Setting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Experts.Blogger.ReadPosts;

public interface IFeature : IUserStoryCore<Request, Response, Settings> {
}

public class Feature(
    IPresenter presenter,
    IValidator validator,
    IRepository repository,
    ISettings<Settings> settings,
    ILog<Feature> logger,
    ITime time) : IFeature {
    public async Task<Response> Run(Request request, CancellationToken token) => await core.Run(request, CoreRun, token);

    private async Task CoreRun(Response response, CancellationToken token) {
        response.Posts = await repository.Read(response.MetaData.Request, token);
        presenter.Handle(response);
    }

    private UserStoryCore<Request, Response, Settings> core = new(validator, settings, logger, time, "");
}

public record Request(string Filter) : RequestCore() { }

public record Response() : ResponseCore<Request>() {
    public IEnumerable<Post>? Posts { get; set; } = [];
}

public record Settings() : SettingsCore("Experts:Blogger:ReadPosts") {
}

public static class Extensions {
    public static IServiceCollection AddReadPosts(this IServiceCollection services) => services
        .AddSettings<Settings>()
        .AddScoped<IFeature, Feature>()
        .AddScoped<IValidator, Validator>()
        .AddScoped<IRepository, Repository>();
}
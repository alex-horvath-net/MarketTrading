using Common.Business.Model;
using Core.Business;
using Core.Business.Model;
using Core.Solutions.Setting;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public interface IFeature : IUserStoryCore<Request, Response, Settings> {
}

public class Feature(
    Presenter presenter,
    IValidator validator,
    IRepository repository,
    ISettings<Settings> settings,
    ILog<Feature> logger,
    ITime time) : UserStoryCore<Request, Response, Settings>(validator, settings, logger, time, nameof(Feature)), IFeature {
    public override async Task Run(Response response, CancellationToken token) {
        response.Posts = await repository.Read(response.MetaData.Request, token);
        presenter.Handle(response);
    }
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
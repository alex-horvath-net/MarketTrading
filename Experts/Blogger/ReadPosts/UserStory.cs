using Common.Business.Model;
using Core.Business;
using Core.Business.Model;
using Core.Solutions.Setting;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public interface IUserStory : IUserStoryCore<Request, Response, Settings> {
}

public class UserStory(
    IPresenter presenter,
    IValidator validator,
    IRepository repository,
    ISettings<Settings> settings,
    ILog<UserStory> logger,
    ITime time) : IUserStory {
    public Task<Response> Run(Request request, CancellationToken token) => core.Run(request, CoreRun, token);

    private async Task CoreRun(Response response, CancellationToken token) {
        response.Posts = await repository.Read(response.MetaData.Request, token);
    }

    private UserStoryCore<Request, Response, Settings> core = new(presenter, validator, settings, logger, time);
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
        .AddScoped<IUserStory, UserStory>()
        .AddScoped<IPresenter, Presenter>()
        .AddScoped<IValidator, Validator>()
        .AddScoped<IRepository, Repository>();
}
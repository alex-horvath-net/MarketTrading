using Common.Business.Model;
using Core.Business.Model;
using Core.Solutions.Setting;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public record Request(string Filter) : RequestCore() { }

public record Response() : ResponseCore<Request>() {
    public IEnumerable<Post>? Posts { get; set; } = [];
}

public record Settings() : SettingsCore("Experts:Blogger:ReadPosts") {
}

public static class Extensions {
    public static IServiceCollection AddReadPosts(this IServiceCollection services) => services
        .AddSettings<Settings>()
        //.AddScoped<IUserStory<Request, Response>, UserStory<Request, Response>>()
        .AddScoped<IPresenter, Presenter>()
        .AddStartUserWorkStep()
        .AddFeatureActivationUserWorkStep()
        .AddValidationUserWorkStep()
        .AddReadPostsUserWorkStep()
        .AddStopUserWorkStep();
}
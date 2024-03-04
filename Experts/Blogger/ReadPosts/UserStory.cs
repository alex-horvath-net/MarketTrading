using Azure.Core;
using Azure;
using Common.Business.Model;
using Core.Business;
using Core.Business.Model;
using Core.Solutions.Setting;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public interface IUserStory : IUserStory<Request, Response> {
}

public class UserStory(
    IEnumerable<IUserWorkStep<Request, Response>> workSteps,
    IPresenter<Request, Response> presenter, ILog<UserStory> log, ITime time) :
    UserStory<Request, Response>(workSteps, presenter, log, time), IUserStory; 

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
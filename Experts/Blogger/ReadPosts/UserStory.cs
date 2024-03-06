using System.Security.Cryptography.Xml;
using Common.Business.Model;
using Core.Business;
using Core.Business.Model;
using Core.Solutions.Setting;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public interface IPresenter : Core.Business.IPresenter<UserStoryRequest, UserStoryResponse> { }

public class Presenter : IPresenter {
    public IEnumerable<Post> ViewModel { get; private set; }
    public void Handle(UserStoryResponse response) {
        var businessModel = response.Posts;

        var solutionModel = businessModel!.Select(p => new Post() {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            Tags = p.Tags.Select(t => new Tag() {
                Id = t.Id,
                Name = t.Name
            })
        });

        ViewModel = solutionModel;
    }
}

public class ControllerAdapter(IUserStory<UserStoryRequest, UserStoryResponse> userStory, Presenter presenter) {
    //It takes the technology free ViewRequestModel (defined in Adapter Layer) from the View (defied in Technology Layer), 
    public async Task<ViewResponseModel> Run(ViewRequestModel viewRequestModel, CancellationToken token) {
        // transforms technology model to business model 
        // transforms ViewRequestModel to UserStoryRequest(defined in UserStory layer) 
        // some property of UserStoryRequest might be wierd, if not provided by the ViewRequestModel.
        var userStoryRequest = new UserStoryRequest(viewRequestModel.Filter);
        //sends it to input port of UserStory(defined in UserStory layer).
        var userStoryResponse = await userStory.Run(userStoryRequest, token);
        // some property of ViewResponseModel might be wierd, if not provided by the UserStoryResponse.
        var viewResponseModel = new ViewResponseModel(presenter.ViewModel);
        return viewResponseModel;
    }

    /// <summary>
    /// Technology free model of the actual view request, which is defied in Technology Layer. 
    /// </summary>
    /// <param name="Filter"></param>
    public record ViewRequestModel(string Filter);

    /// <summary>
    /// Technology free, convinient model of the actual view response, which is defied in Technology Layer. 
    /// </summary>
    /// <param name="Posts"></param>
    public record ViewResponseModel(IEnumerable<Post> Posts);
}

public record UserStoryRequest(string Filter) : RequestCore() { }

public record UserStoryResponse() : ResponseCore<UserStoryRequest>() {
    public IEnumerable<Post>? Posts { get; set; } = [];
}

public record UserStorySettings() : SettingsCore("Experts:Blogger:ReadPosts") {
}

public static class Extensions {
    public static IServiceCollection AddReadPosts(this IServiceCollection services) => services
        .AddSettings<UserStorySettings>()
        //.AddScoped<IUserStory<UserStoryRequest, Response>, UserStory<UserStoryRequest, Response>>()
        .AddScoped<IPresenter, Presenter>()
        .AddStartUserWorkStep()
        .AddFeatureActivationUserWorkStep()
        .AddValidationUserWorkStep()
        .AddReadPostsUserWorkStep()
        .AddStopUserWorkStep();
}
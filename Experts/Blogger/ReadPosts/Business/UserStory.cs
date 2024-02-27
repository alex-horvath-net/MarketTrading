using Core.Business;
using Experts.Blogger.ReadPosts.Business.Model;

namespace Experts.Blogger.ReadPosts.Business;

public class UserStory(
    IPresenter presenter, 
    IValidator validator, 
    IRepository repository, 
    ISettings<Settings> settings,
    ILog<UserStory> logger, 
    ITime time) : UserStoryCore<Request, Response, Settings>(validator, settings,  logger, time, nameof(UserStory)), IUserStory {
    public override async Task Run(Response response, CancellationToken token) {
        response.Posts = await repository.Read(response.MetaData.Request, token);
        presenter.Map(response);
    }
}
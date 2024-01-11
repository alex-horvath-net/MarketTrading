using Common.Models.DataModel;
using Core.UserStory;


namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class Scope(ISolution solution) : IScope<Request, Response> {
    public async Task Run(Response response, CancellationToken token) {

        var solutionModel = await solution.Read(response.Request, token);
        var domainModel = solutionModel.Select(ToDomainModel);
        response.Posts = domainModel;
    }

    private Common.Models.DomainModel.Post ToDomainModel(Post solutionModel) => new() {
        Title = solutionModel.Title,
        Content = solutionModel.Content
    };
}


public interface ISolution {
    Task<IEnumerable<Post>> Read(Request Request, CancellationToken token);
}
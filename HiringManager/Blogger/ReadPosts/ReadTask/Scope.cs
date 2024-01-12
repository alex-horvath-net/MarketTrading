using Core.ExpertStory;

namespace BusinessExperts.Blogger.ReadPosts.ReadTask;

public class Scope(ISolution solution) : IScope<Request, Response> {
    public async Task Run(Response response, CancellationToken token) {

        var solutionModel = await solution.Read(response.Request, token);
        var scopeModel = solutionModel.Select(ToScopeModel);
        response.Posts = scopeModel;
    }

    private Common.Scope.ScopeModel.Post ToScopeModel(Common.Solutions.Data.MainDB.DataModel.Post solutionModel) => new() {
        Title = solutionModel.Title,
        Content = solutionModel.Content
    };
}


public interface ISolution {
    Task<IEnumerable<Common.Solutions.Data.MainDB.DataModel.Post>> Read(Request Request, CancellationToken token);
}
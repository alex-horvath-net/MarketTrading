using Common.Solutions.DataModel.ValidationModel;
using Core.ExpertStory;
using Core.ExpertStory.DomainModel;
using Experts.Blogger.ReadPosts;

namespace Experts.Blogger.ReadPosts.ValidationTask;

public class Scope(ISolution solution) : IScope<Request, Response> {
    public async Task Run(Response response, CancellationToken token) {
        var solutionModel = await solution.Validate(response.Request, token);
        var scopeModel = solutionModel.Select(ToScopeModel);
        response.Validations = scopeModel;
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }

    private Validation ToScopeModel(ValidationIssue solutionModel) =>
        Validation.Failed(solutionModel.ErrorCode, solutionModel.ErrorMessage);
}

public interface ISolution {
    Task<IEnumerable<ValidationIssue>> Validate(Request request, CancellationToken token);
}

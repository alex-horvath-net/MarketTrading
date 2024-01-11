using Common.Models.ValidationModel;
using Core.UserStory;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class Scope(ISolution solution) : IScope<Request, Response>
{
    public async Task Run(Response response, CancellationToken token)
    {
        var solutionModel = await solution.Validate(response.Request, token);
        var domainModel = solutionModel.Select(ToDomainModel);
        response.Validations = domainModel;
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }

    private Core.UserStory.DomainModel.Validation ToDomainModel(Validation solutionModel) => 
        Core.UserStory.DomainModel.Validation.Failed(solutionModel.ErrorCode, solutionModel.ErrorMessage);
}

public interface ISolution {
    Task<IEnumerable<Common.Models.ValidationModel.Validation>> Validate(Request request, CancellationToken token);
}

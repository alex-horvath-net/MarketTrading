using Core.Sockets.ValidationModel;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class SolutionExpert(ISolution solution) : ISolutionExpert {
    public async Task<IEnumerable<ValidationDomainModel>> Validate(Request request, CancellationToken token) {
        var expertModel = await solution.Validate(request, token);
        var scopeModel = expertModel.Select(result => ValidationDomainModel.Failed(result.ErrorCode, result.ErrorMessage));
        return scopeModel;
    }
}

public interface ISolution {
    Task<IEnumerable<ValidationSolutionExpertModel>> Validate(Request request, CancellationToken token);
}


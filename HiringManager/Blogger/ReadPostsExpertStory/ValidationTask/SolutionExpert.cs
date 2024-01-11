using Core.Sockets.ValidationModel;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class SolutionExpert(ISolution solution) : ISolutionExpert {
    public async Task<IEnumerable<ValidationDomainModel>> Validate(Request request, CancellationToken token) {
        var expertModel = await solution.Validate(request, token);
        var scopeModel = expertModel.Select(result => ValidationDomainModel.Failed(result.ErrorCode, result.ErrorMessage));
        return scopeModel;
    }

    public async Task<IEnumerable<ValidationDomainModel>> Validate2(Request request, CancellationToken token) =>
        from expertModel in await solution.Validate(request, token)
        select ValidationDomainModel.Failed(expertModel.ErrorCode, expertModel.ErrorMessage);
}

public interface ISolution {
    Task<IEnumerable<ValidationSolutionExpertModel>> Validate(Request request, CancellationToken token);
}


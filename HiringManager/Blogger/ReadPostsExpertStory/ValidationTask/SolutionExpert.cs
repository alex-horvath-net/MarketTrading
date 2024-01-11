using Core.Sockets.ValidationModel;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public interface ISolution {
    Task<IEnumerable<ValidationSocketModel>> Validate(Request request, CancellationToken token);
}

public class SolutionExpert(ISolution plugin) : ISolutionExpert {
    public async Task<IEnumerable<ValidationDomainModel>> Validate(Request request, CancellationToken token) {
        var socketModel = await plugin.Validate(request, token);
        var domainModel = socketModel.Select(result => ValidationDomainModel.Failed(result.ErrorCode, result.ErrorMessage));
        return domainModel;
    }
}



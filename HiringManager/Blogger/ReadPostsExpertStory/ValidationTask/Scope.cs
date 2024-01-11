using Core.UserStory;
using Core.UserStory.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class Scope(ISolutionExpert expert) : IScope<Request, Response>
{
    public async Task Run(Response response, CancellationToken token)
    {
        response.Validations = await expert.Validate(response.Request, token);
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }
}

public interface ISolutionExpert {
    Task<IEnumerable<ValidationDomainModel>> Validate(Request request, CancellationToken token);
}

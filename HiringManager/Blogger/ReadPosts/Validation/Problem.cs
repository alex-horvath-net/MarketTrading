using Core.Story;
using Core.Story.Model;
using Experts.Blogger.ReadPosts.Model;

namespace Experts.Blogger.ReadPosts.Validation;

public class Problem(ISolution solution) : IProblem<Model.Request, Response> {
    public async Task Run(Response response, CancellationToken token) {
        response.Validations = await solution.Validate(response.Request, token);
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }
}

public interface ISolution
{
    Task<IEnumerable<ValidationResult>> Validate(Model.Request request, CancellationToken token);
}
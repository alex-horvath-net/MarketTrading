using Core.ExpertStory;

namespace Experts.Blogger.ReadPosts.Validation;

public class Problem(ISolution solution) : IProblem<Request, Response> {
    public async Task Run(Response response, CancellationToken token) {
        response.Validations = await solution.Validate(response.Request, token);
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }
}


public interface ISolution
{
    Task<IEnumerable<Core.ExpertStory.StoryModel.ValidationResult>> Validate(Request request, CancellationToken token);
}
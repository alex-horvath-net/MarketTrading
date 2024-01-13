using Core.ExpertStory;

namespace Experts.Blogger.ReadPosts.Validation;

public class ExpertTask(ISolution solution) : IExpertTask<Request, Response> {
    public async Task Run(Response response, CancellationToken token) {
        response.Validations = await solution.Validate(response.Request, token);
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }
}

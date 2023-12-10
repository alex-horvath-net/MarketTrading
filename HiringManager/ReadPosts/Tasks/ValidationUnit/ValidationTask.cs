using Blogger.ReadPosts.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.Tasks.ValidationUnit;

public class ValidationTask(IValidationAdapter validation) : ITask
{
    public async Task Run(Response response, CancellationToken cancellation)
    {
        response.Validations = await validation.Validate(response.Request, cancellation);
        response.Stopped = response.Validations.Any(x => !x.IsSuccess);
    }
}
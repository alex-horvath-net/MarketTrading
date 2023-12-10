using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TaskLayer.ValidationUnit;

public class ValidationTask(IValidationAdapter validation) : ITask
{
    public async Task Run(Response response, CancellationToken cancellation)
    {
        response.Validations = await validation.Validate(response.Request, cancellation);
        response.Stopped = response.Validations.Any(x => !x.IsSuccess);
    }
}
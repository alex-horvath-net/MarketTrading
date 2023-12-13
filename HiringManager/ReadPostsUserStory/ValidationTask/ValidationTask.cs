using Core.UserStory;

namespace BloggerUserRole.ReadPostsUserStory.ValidationTask;

public class ValidationTask(IValidationSocket socket) : Core.UserStory.ITask<Request, Response>
{
    public async Task Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        response.Stopped = response.Validations.Any(x => !x.IsSuccess);
    }
}

public interface IValidationSocket
{
    Task<IEnumerable<Core.UserStory.ValidationResult>> Validate(Request request, CancellationToken token);
}
using Core.UserStory;

namespace BloggerUserRole.ReadPostsUserStory.ValidationTask;

public class ValidationTask(IValidationSocket socket) : ITask<Request, Response>
{
    public async Task Run(Response response, CancellationToken cancellation)
    {
        response.Validations = await socket.Validate(response.Request, cancellation);
        response.Stopped = response.Validations.Any(x => !x.IsSuccess);
    }
}

public interface IValidationSocket
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation);
}
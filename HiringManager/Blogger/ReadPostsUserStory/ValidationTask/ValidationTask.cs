namespace Users.Blogger.ReadPostsUserStory.ValidationTask;

public class ValidationTask(ValidationTask.IValidationSocket socket) : IUserTask<Request, Response>
{
    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        var hasValidationIssue = response.Validations.Any(x => !x.IsSuccess);
        return hasValidationIssue;
    }

    public interface IValidationSocket
    {
        Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token);

    }
}
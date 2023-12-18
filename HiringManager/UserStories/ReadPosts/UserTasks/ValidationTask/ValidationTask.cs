using Core.UserStory;

namespace Blogger.UserStories.ReadPosts.UserTasks.ValidationTask;

public class ValidationTask(IValidationSocket socket) : IUserTask<Request, Response>
{
    public async Task<bool> Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        return response.Validations.Any(x => !x.IsSuccess);
    }
}

public interface IValidationSocket
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token);
}
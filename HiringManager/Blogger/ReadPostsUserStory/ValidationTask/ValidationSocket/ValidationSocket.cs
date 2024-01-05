using Core.Enterprise.Sockets.Validation;
using Core.Enterprise.UserStory;

namespace Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket;

public class ValidationSocket(IValidationPlugin plugin) : IValidationSocket
{
    public async Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token)
    {
        var socketModel = await plugin.Validate(request, token);
        var userStoryModel = socketModel.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return userStoryModel;
    }
}

public interface IValidationPlugin
{
    Task<IEnumerable<ValidationFailure>> Validate(Request request, CancellationToken token);
}
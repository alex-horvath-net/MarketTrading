using Core.UserStory;

namespace BloggerUserRole.ReadPostsUserStory.ValidationTask.ValidationSocket;

public class ValidationSocket(IValidationPlugin plugin) : IValidationSocket
{
    public async Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation)
    {
        var adapter = await plugin.Validate(request, cancellation);
        var business = adapter.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return business;
    }
}

public interface IValidationPlugin
{
    Task<IEnumerable<Core.Sockets.Validation.ValidationResult>> Validate(Request request, CancellationToken cancellation);
}
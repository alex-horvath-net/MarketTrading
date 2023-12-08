namespace Blogger.ReadPosts.Adapters;

public class Validation(
    IValidation validatorPlugin) : Tasks.IValidation
{
    public async Task<IEnumerable<Sys.UserStory.ValidationResult>> Validate(UserStory.Request request, CancellationToken cancellation)
    {
        var adapter = await validatorPlugin.Validate(request, cancellation);
        var business = adapter.Select(result => Sys.UserStory.ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return business;
    }
}

public interface IValidation
{
    Task<IEnumerable<Sys.Adapters.ValidationResult>> Validate(UserStory.Request request, CancellationToken cancellation);
}

//--Test--------------------------------------------------
using BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Principals.AdaptersLayer;
using Principals.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.AdaptersLayer;

public class Validation(
    IValidation validatorPlugin) : IValidationAdapter
{
    public async Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation)
    {
        var adapter = await validatorPlugin.Validate(request, cancellation);
        var business = adapter.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return business;
    }
}

public interface IValidation
{
    Task<IEnumerable<Principals.AdaptersLayer.ValidationResult>> Validate(Request request, CancellationToken cancellation);
}

//--Test--------------------------------------------------
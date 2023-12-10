using Blogger.ReadPosts.Tasks.ValidationUnit;
using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Core.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.Adapters.ValidationUnit;

public class ValidationAdapter(
    IValidationPlugin validatorPlugin) : IValidationAdapter
{
    public async Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation)
    {
        var adapter = await validatorPlugin.Validate(request, cancellation);
        var business = adapter.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return business;
    }
}
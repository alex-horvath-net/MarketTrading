using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Core.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.AdaptersLayer.ValidationUnit;

public interface IValidationPlugin
{
    Task<IEnumerable<Core.AdaptersLayer.ValidationUnit.ValidationResult>> Validate(UserStoryLayer.UserStoryUnit.Request request, CancellationToken cancellation);
}


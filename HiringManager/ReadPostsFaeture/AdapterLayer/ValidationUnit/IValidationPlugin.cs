using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Core.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.AdapterLayer.ValidationUnit;

public interface IValidationPlugin
{
    Task<IEnumerable<Core.AdapterLayer.ValidationUnit.ValidationResult>> Validate(Request request, CancellationToken cancellation);
}


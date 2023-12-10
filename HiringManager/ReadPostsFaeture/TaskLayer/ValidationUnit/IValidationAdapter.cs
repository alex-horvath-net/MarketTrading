using Core.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TaskLayer.ValidationUnit;

public interface IValidationAdapter
{
    Task<IEnumerable<ValidationResult>> Validate(UserStoryLayer.UserStoryUnit.Request request, CancellationToken cancellation);
}

using Core.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;

public interface IValidationAdapter
{
    Task<IEnumerable<ValidationResult>> Validate(UserStoryLayer.UserStoryUnit.Request request, CancellationToken cancellation);
}

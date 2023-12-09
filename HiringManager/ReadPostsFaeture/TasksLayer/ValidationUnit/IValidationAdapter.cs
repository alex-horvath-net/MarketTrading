using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Principals.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;

public interface IValidationAdapter
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation);
}

//--Test--------------------------------------------------
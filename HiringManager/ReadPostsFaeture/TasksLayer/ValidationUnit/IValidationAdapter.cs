using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Core.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TasksLayer.ValidationUnit;

public interface IValidationAdapter
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation);
}

//--Test--------------------------------------------------
using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Core.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.Tasks.ValidationUnit;

public interface IValidationAdapter
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation);
}

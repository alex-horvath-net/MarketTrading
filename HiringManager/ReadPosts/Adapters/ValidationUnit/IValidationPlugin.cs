using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Core.Adapters.ValidationUnit;

namespace Blogger.ReadPosts.Adapters.ValidationUnit;

public interface IValidationPlugin
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation);
}


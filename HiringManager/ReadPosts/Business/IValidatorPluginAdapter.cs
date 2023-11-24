using Core.Business.ValidationModel;

namespace Blogger.ReadPosts.Business;

public interface IValidatorPluginAdapter
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation);
}
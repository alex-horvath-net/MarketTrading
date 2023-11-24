using Blogger.ReadPosts.Business;
using Core.PluginAdapters.ValidationModel;

namespace Blogger.ReadPosts.PluginAdapters;

public interface IValidatorPlugin
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation);
}
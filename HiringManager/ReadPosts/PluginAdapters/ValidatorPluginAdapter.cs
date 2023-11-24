using Blogger.ReadPosts.Business;
using Core.Business.ValidationModel;

namespace Blogger.ReadPosts.PluginAdapters;

public class ValidatorPluginAdapter(
    PluginAdapters.IValidatorPlugin validatorPlugin) : IValidatorPluginAdapter
{
    public async Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken cancellation)
    {
        var adapter = await validatorPlugin.Validate(request, cancellation);
        var business = adapter.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return business;
    }
}

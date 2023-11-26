
namespace Blogger.ReadPosts.PluginAdapters;

public class ValidatorPluginAdapter(
    PluginAdapters.IValidatorPlugin validatorPlugin) : Business.IValidatorPluginAdapter
{
    public async Task<IEnumerable<Core.Business.ValidationResult>> Validate(Business. Request request, CancellationToken cancellation)
    {
        var adapter = await validatorPlugin.Validate(request, cancellation);
        var business = adapter.Select(result => Core.Business.ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return business;
    }
}

public interface IValidatorPlugin
{
    Task<IEnumerable<Core.PluginAdapters.ValidationResult>> Validate(Business.Request request, CancellationToken cancellation);
}

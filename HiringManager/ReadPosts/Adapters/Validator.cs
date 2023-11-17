using Blogger.ReadPosts.Business;
using Shared.Business;

namespace Blogger.ReadPosts.Adapters;

public class Validator : IValidator
{
    public async Task<ValidationResult> Validate(Request request, CancellationToken token) =>
        request == null ? ValidationResult.Failed("Request", "Can not be null.") :
        ValidationResult.Success();

    public string? Code { get; }
    public string? Message { get; }

}

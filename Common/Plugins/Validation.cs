using FluentValidation;

namespace Core.Plugins;

public abstract class FluentValidator<T> : AbstractValidator<T>
{
    public async Task<IEnumerable<Core.PluginAdapters.ValidationResult>> Validate(T request, CancellationToken cancellation)
    {
        var technology = await ValidateAsync(request, cancellation);
        var adapter = technology.Errors.Select(error => new Core.PluginAdapters.ValidationResult(
            error.PropertyName,
            error.ErrorCode,
            error.ErrorMessage,
            error.Severity.ToString()));
        return adapter;
    }
}

using Core.PluginAdapters;
using FluentValidation;

namespace Core.Plugins.Validation;

public abstract class FluentValidator<T> : AbstractValidator<T>
{
    public async Task<IEnumerable<ValidationResult>> Validate(T request, CancellationToken cancellation)
    {
        var technology = await ValidateAsync(request, cancellation);
        var adapter = technology.Errors.Select(error => new ValidationResult(
            error.PropertyName,
            error.ErrorCode,
            error.ErrorMessage,
            error.Severity.ToString()));
        return adapter;
    }
}

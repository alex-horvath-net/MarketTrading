using Core.Sockets.Validation;

namespace Core.Plugins.Validation;

public abstract class FluentValidator<T> : FluentValidation.AbstractValidator<T>
{
    public async Task<IEnumerable<ValidationResult>> Validate(T request, CancellationToken token)
    {
        var pluginModel = await ValidateAsync(request, token);

        var socketModel = pluginModel.Errors.Select(error => new ValidationResult(
            error.PropertyName,
            error.ErrorCode,
            error.ErrorMessage,
            error.Severity.ToString()));

        return socketModel;
    }
}

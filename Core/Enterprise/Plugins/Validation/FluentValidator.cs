using Core.Enterprise.Sockets.Validation;
using FluentValidation;

namespace Core.Enterprise.Plugins.Validation;

public abstract class FluentValidator<T> : AbstractValidator<T>
{
    public async Task<IEnumerable<ValidationFailure>> Validate(T request, CancellationToken token)
    {
        var pluginModel = await ValidateAsync(request, token);
        var socketModel = pluginModel.Errors.Select(ToSocketModel);
        return socketModel;
    }

    private ValidationFailure ToSocketModel(FluentValidation.Results.ValidationFailure plugin) => new(
        plugin.PropertyName,
        plugin.ErrorCode,
        plugin.ErrorMessage,
        plugin.Severity.ToString());
}



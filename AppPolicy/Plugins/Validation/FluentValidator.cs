using AppPolicy.Sockets.ValidationModel;
using AppPolicy;
using FluentValidation;
using FluentValidation.Results;

namespace AppPolicy.Plugins.Validation;

public abstract class FluentValidator<T> : AbstractValidator<T> {
    public async Task<IEnumerable<ValidationSocketModel>> Validate(T request, CancellationToken token) {
        var pluginModel = await ValidateAsync(request, token);
        var socketModel = pluginModel.Errors.Select(ToSocketModel);
        return socketModel;
    }

    private ValidationSocketModel ToSocketModel(ValidationFailure plugin) => new(
        plugin.PropertyName,
        plugin.ErrorCode,
        plugin.ErrorMessage,
        plugin.Severity.ToString());
}



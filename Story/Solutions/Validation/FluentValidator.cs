using Common.Business;
using Core;
using FluentValidation;

namespace Common.Solutions.Validation;

public abstract class FluentValidator<T> : AbstractValidator<T> {
    public async Task<IEnumerable<ValidationResult>> Validate(T request, CancellationToken token) {
        var solutionModel = await ValidateAsync(request, token);

        var businessModel = solutionModel
            .Errors
            .Select(model => ValidationResult.Failed(model.ErrorCode, model.ErrorMessage));

        return businessModel;
    }
}



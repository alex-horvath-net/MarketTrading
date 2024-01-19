using Core;
using FluentValidation;

namespace Core.Solutions.Validation;

public abstract class FluentValidator<T> : AbstractValidator<T> {
    public async Task<IEnumerable<Story.Model.ValidationResult>> Validate(T request, CancellationToken token) {
        var solutionModel = await ValidateAsync(request, token);

        var problemModel = solutionModel
            .Errors
            .Select(model => Story.Model.ValidationResult.Failed(model.ErrorCode, model.ErrorMessage));

        return problemModel;
    }
}



using Azure.Core;
using Core;
using Core.Business;
using FluentValidation;

namespace Core.Solutions.Validation;

public class ValidationCore<TRequest> : AbstractValidator<TRequest>, IValidation<TRequest> where TRequest : Business.Request {
    public async Task<IEnumerable<ValidationResult>> Validate(TRequest request, CancellationToken token) {
        var solutionModel = await ValidateAsync(request, token);

        var businessModel = solutionModel
            .Errors
            .Select(model => ValidationResult.Failed(model.ErrorCode, model.ErrorMessage));

        return businessModel;
    }
}



using Core.Solutions.Validation;
using FluentValidation;

namespace Experts.Blogger.ReadPosts;

public class Validation : ValidationCore<Request>, IValidator
{
    public Validation() {
        RuleFor(request => request.Title)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Title)}' can not be empty if '{nameof(request.Content)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator);

        RuleFor(request => request.Content)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Content)}' can not be empty if '{nameof(request.Title)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator);
    }
}

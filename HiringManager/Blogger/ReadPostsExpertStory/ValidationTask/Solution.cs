using Core.Sockets.ValidationModel;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ValidationTask;

public class Solution : AbstractValidator<Request>, ISolution {
    public Solution() {
        RuleFor(request => request.Title)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Title)}' can not be empty if '{nameof(request.Content)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator);

        RuleFor(request => request.Content)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Content)}' can not be empty if '{nameof(request.Title)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator);
    }

    public async Task<IEnumerable<ValidationSolutionExpertModel>> Validate(Request request, CancellationToken token) {
        var solutionModel = await ValidateAsync(request, token);
        var expertModel = solutionModel.Errors.Select(ToExpertModel);
        return expertModel;
    }

    private ValidationSolutionExpertModel ToExpertModel(ValidationFailure solutionModel) => new(
        solutionModel.PropertyName,
        solutionModel.ErrorCode,
        solutionModel.ErrorMessage,
        solutionModel.Severity.ToString());
}

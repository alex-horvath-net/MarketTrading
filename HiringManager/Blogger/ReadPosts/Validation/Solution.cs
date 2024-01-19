using Core;
using FluentValidation;

namespace Experts.Blogger.ReadPosts.Validation;

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

    public async Task<IEnumerable<Core.ExpertStory.StoryModel.ValidationResult>> Validate(Request request, CancellationToken token) {
        var solutionModel = await ValidateAsync(request, token);
        
        var problemModel = solutionModel
            .Errors
            .Select(model => Core.ExpertStory.StoryModel.ValidationResult.Failed(model.ErrorCode, model.ErrorMessage));

        return problemModel;
    }
}



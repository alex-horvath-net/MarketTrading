using Common.Solutions.DataModel.ValidationModel;
using FluentValidation;
using FluentValidation.Results;

namespace Experts.Blogger.ReadPosts.Validation;

public class Solution : AbstractValidator<Request>, ISolution
{
    public Solution()
    {
        RuleFor(request => request.Title)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Title)}' can not be empty if '{nameof(request.Content)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator);

        RuleFor(request => request.Content)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Content)}' can not be empty if '{nameof(request.Title)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator);
    }

    public async Task<IEnumerable<Core.ExpertStory.StoryModel.ValidationResult>> Validate(Request request, CancellationToken token)
    {
        var technology = await ValidateAsync(request, token);
        var model = technology.Errors.Select(ToSolutionModel);
        var domain = model.Select(ToTaskModel);
        return domain;
    }

    private ValidationIssue ToSolutionModel(ValidationFailure technologyModel) => new(
        technologyModel.PropertyName,
        technologyModel.ErrorCode,
        technologyModel.ErrorMessage,
        technologyModel.Severity.ToString());

    private Core.ExpertStory.StoryModel.ValidationResult ToTaskModel(ValidationIssue solutionModel) =>
        Core.ExpertStory.StoryModel.ValidationResult.Failed(solutionModel.ErrorCode, solutionModel.ErrorMessage);
}



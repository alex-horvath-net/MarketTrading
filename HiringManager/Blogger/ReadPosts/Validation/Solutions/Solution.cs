using Common.Solutions.DataModel.ValidationModel;
using FluentValidation;
using FluentValidation.Results;

namespace Experts.Blogger.ReadPosts.Validation.Solutions;

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

    public async Task<IEnumerable<Core.ExpertStory.DomainModel.Validation>> Validate(Request request, CancellationToken token)
    {
        var technologyModel = await ValidateAsync(request, token);
        var solutionModel = technologyModel.Errors.Select(ToSolutionModel);
        var scopeModel = solutionModel.Select(ToScopeModel);
        return scopeModel;
    }

    private ValidationIssue ToSolutionModel(ValidationFailure technologyModel) => new(
        technologyModel.PropertyName,
        technologyModel.ErrorCode,
        technologyModel.ErrorMessage,
        technologyModel.Severity.ToString());

    private Core.ExpertStory.DomainModel.Validation ToScopeModel(ValidationIssue solutionModel) =>
        Core.ExpertStory.DomainModel.Validation.Failed(solutionModel.ErrorCode, solutionModel.ErrorMessage);
}

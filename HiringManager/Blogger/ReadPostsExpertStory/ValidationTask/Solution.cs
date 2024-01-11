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

    public async Task<IEnumerable<ValidationSocketModel>> Validate(Request request, CancellationToken token) {
        var pluginModel = await ValidateAsync(request, token);
        var socketModel = pluginModel.Errors.Select(ToSocketModel);
        return socketModel;
    }

    private ValidationSocketModel ToSocketModel(ValidationFailure pluginModel) => new(
        pluginModel.PropertyName,
        pluginModel.ErrorCode,
        pluginModel.ErrorMessage,
        pluginModel.Severity.ToString());
}

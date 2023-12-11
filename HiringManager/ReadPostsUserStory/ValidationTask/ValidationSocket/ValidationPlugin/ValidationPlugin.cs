
using FluentValidation;

namespace BloggerUserRole.ReadPostsUserStory.ValidationTask.ValidationSocket.ValidationPlugin;

public class ValidationPlugin : Core.Plugins.Validation.FluentValidator<Request>, IValidationPlugin
{
    public ValidationPlugin() =>
        RuleFor(request => request)
        .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
        .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
}

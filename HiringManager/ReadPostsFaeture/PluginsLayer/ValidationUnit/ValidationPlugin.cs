using BloggerUserRole.ReadPostsFaeture.AdaptersLayer.ValidationUnit;
using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Core.PluginsLayer.ValidationUnit;
using FluentValidation;

namespace BloggerUserRole.ReadPostsFaeture.PluginsLayer.ValidationUnit;

public class ValidationPlugin : FluentValidator<Request>, IValidationPlugin
{
    public ValidationPlugin() =>
        RuleFor(request => request)
        .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
        .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
}

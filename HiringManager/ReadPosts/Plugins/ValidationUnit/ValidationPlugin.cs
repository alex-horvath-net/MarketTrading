using Blogger.ReadPosts.Adapters.ValidationUnit;
using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Core.Plugins.ValidationUnit;
using FluentValidation;

namespace Blogger.ReadPosts.Plugins.ValidationUnit;

public class ValidationPlugin : FluentValidator<Request>, IValidationPlugin
{
    public ValidationPlugin() =>
        RuleFor(request => request)
        .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
        .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
}

using Core.Enterprise.Plugins.Validation;
using FluentValidation;

namespace Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask.Sockets.ValidationSocket.Plugins.ValidationPlugin;

public class ValidationPlugin : FluentValidator<Request>, IValidationPlugin
{
    public ValidationPlugin() => RuleFor(request => request)
        .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
        .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
}

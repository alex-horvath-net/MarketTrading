using Core.Plugins;
using FluentValidation;

namespace Blogger.ReadPosts.Plugins;

public class ValidationPlugin : FluentValidator<Business.Request>, PluginAdapters.IValidationPlugin
{
    public ValidationPlugin() => base
        .RuleFor(request => request)
        .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
        .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
}

using FluentValidation;

namespace Blogger.ReadPosts.Plugins;

public class ValidatorPlugin : Core.Plugins.Validation.FluentValidator<Business.Request>, PluginAdapters.IValidatorPlugin
{
    public ValidatorPlugin() => base
        .RuleFor(request => request)
        .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
        .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
}

using Blogger.ReadPosts.Business;
using Blogger.ReadPosts.PluginAdapters;
using Core.Plugins.Validation;
using FluentValidation;

namespace Blogger.ReadPosts.Plugins;

public class ValidatorPlugin : FluentValidator<Request>, IValidatorPlugin
{
    public ValidatorPlugin()
    {
        RuleFor(request => request)
            .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
            .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
    }
}

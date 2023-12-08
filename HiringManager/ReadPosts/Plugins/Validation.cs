using FluentValidation;

namespace Blogger.ReadPosts.Plugins;

public class Validation : Sys.Plugins.FluentValidator<UserStory.Request>, Adapters.IValidation
{
    public Validation() => base
        .RuleFor(request => request)
        .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
        .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
}
                                                                                                                             
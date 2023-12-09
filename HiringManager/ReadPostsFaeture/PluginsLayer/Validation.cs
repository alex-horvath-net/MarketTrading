using BloggerUserRole.ReadPostsFaeture.AdaptersLayer;
using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Core.PluginsLayer.ValidationUnit;
using FluentValidation;

namespace BloggerUserRole.ReadPostsFaeture.PluginsLayer;

public class Validation : FluentValidator<Request>, IValidation
{
    public Validation() =>
        RuleFor(request => request)
        .Must(request => !string.IsNullOrWhiteSpace(request.Title) || !string.IsNullOrWhiteSpace(request.Content))
        .WithMessage(request => $"Either {nameof(request.Title)} or {nameof(request.Content)} must be provided.");
}

//--Test--------------------------------------------------
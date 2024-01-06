
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket;
using Users.Blogger.ReadPostsUserStory.ValidationUserTask.ValidationSocket;

namespace Users.Blogger.ReadPostsUserStory.ValidationUserTask.ValidationSocket.ValidationPlugin;

public class Plugin : FluentValidator<Request>, Socket.IValidationPlugin
{
    public Plugin()
    {
        RuleFor(request => request.Title)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Title)}' can not be empty if '{nameof(request.Content)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator);

        RuleFor(request => request.Content)
            .NotEmpty().When(request => string.IsNullOrWhiteSpace(request.Title), ApplyConditionTo.CurrentValidator)
            .WithMessage(request => $"'{nameof(request.Content)}' can not be empty if '{nameof(request.Title)}' is empty.")
            .MinimumLength(3).When(request => !string.IsNullOrWhiteSpace(request.Content), ApplyConditionTo.CurrentValidator);
    }
}


public static class PluginExtensions
{
    public static IServiceCollection AddValidationPlugin(this IServiceCollection services) => services
        .AddScoped<Socket.IValidationPlugin, Plugin>();
}
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.ReadPostsUserStory;

public class ValidationTask(IValidationSocket socket) : IUserTask<Request, Response>
{
    public async Task Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }
}

public interface IValidationSocket
{
    Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token);
}

public class ValidationSocket(IValidationPlugin plugin) : IValidationSocket
{
    public async Task<IEnumerable<ValidationResult>> Validate(Request request, CancellationToken token)
    {
        var socketModel = await plugin.Validate(request, token);
        var userStoryModel = socketModel.Select(result => ValidationResult.Failed(result.ErrorCode, result.ErrorMessage));
        return userStoryModel;
    }
}

public interface IValidationPlugin
{
    Task<IEnumerable<ValidationFailure>> Validate(Request request, CancellationToken token);
}

public class ValidationPlugin : FluentValidator<Request>, IValidationPlugin
{
    public ValidationPlugin()
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

public static class ValidationExtensions 
{
    public static IServiceCollection AddValidationTask(this IServiceCollection services) => services
        .AddScoped<IUserTask<Request, Response>, ValidationTask>()
        .AddScoped<IValidationSocket, ValidationSocket>()
        .AddScoped<IValidationPlugin, ValidationPlugin>();
}
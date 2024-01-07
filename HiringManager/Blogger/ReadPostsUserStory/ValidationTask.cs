using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace Users.Blogger.ReadPostsUserStory;

public class ValidationPlugin : AbstractValidator<Request>, IValidationPlugin
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

    public async Task<IEnumerable<ValidationSocketModel>> Validate(Request request, CancellationToken token)
    {
        var pluginModel = await base.ValidateAsync(request, token);
        var socketModel = pluginModel.Errors.Select(ToSocketModel);
        return socketModel;
    }

    private ValidationSocketModel ToSocketModel(ValidationFailure pluginModel) => new(
        pluginModel.PropertyName,
        pluginModel.ErrorCode,
        pluginModel.ErrorMessage,
        pluginModel.Severity.ToString());
}


public interface IValidationPlugin
{
    Task<IEnumerable<ValidationSocketModel>> Validate(Request request, CancellationToken token);
}


public class ValidationSocket(IValidationPlugin plugin) : IValidationSocket
{
    public async Task<IEnumerable<SUS.DomainModel.Validation>> Validate(Request request, CancellationToken token)
    {
        var socketModel = await plugin.Validate(request, token);
        var userStoryModel = socketModel.Select(result => SUS.DomainModel.Validation.Failed(result.ErrorCode, result.ErrorMessage));
        return userStoryModel;
    }
}


public interface IValidationSocket
{
    Task<IEnumerable<SUS.DomainModel.Validation>> Validate(Request request, CancellationToken token);
}


public class ValidationTask(IValidationSocket socket) : SUS.IUserTask<Request, Response>
{
    public async Task Run(Response response, CancellationToken token)
    {
        response.Validations = await socket.Validate(response.Request, token);
        response.Terminated = response.Validations.Any(x => !x.IsSuccess);
    }
}



public static class ValidationExtensions
{
    public static IServiceCollection AddValidationTask(this IServiceCollection services) => services
        .AddScoped<SUS.IUserTask<Request, Response>, ValidationTask>()
        .AddScoped<IValidationSocket, ValidationSocket>()
        .AddScoped<IValidationPlugin, ValidationPlugin>();
}
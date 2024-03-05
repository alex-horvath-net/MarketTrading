using Core;
using Core.Business.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Solutions.Validation;

public class Validator<TRequest> : AbstractValidator<TRequest>, Business.IValidator<TRequest> where TRequest : RequestCore {
    public async Task<IEnumerable<Result>> Validate(TRequest request, CancellationToken token) {
        var solutionModel = await ValidateAsync(request, token);

        var businessModel = solutionModel
            .Errors
            .Select(model => new Failed(model.ErrorCode, model.ErrorMessage));

        return businessModel;
    }
}

//public record ValidationIssue(
//    string PropertyName,
//    string ErrorCode,
//    string ErrorMessage,
//    string Severity);

public static class Extensions {
    public static IServiceCollection AddFluentValidation(this IServiceCollection services) {
        services.AddScoped(typeof(IValidator<>), typeof(Validator<>));
        return services;
    }

    public static bool HasFailed(this IEnumerable<Result> results) => results.Any(x => x.IsFailed);
}


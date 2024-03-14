using Core.Business;
using Core.Solutions.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts.WorkSteps;

public class Validation(Validation.IValidator validator) : UserWorkStep<UserStoryRequest, UserStoryResponse> {
    public async Task<bool> Run(UserStoryResponse response, CancellationToken token) {
        response.MetaData.RequestIssues = await validator.Validate(response.MetaData.Request, token);
        return response.MetaData.RequestIssues.HasFailed();
    }

    public interface IValidator : Core.Business.IValidator<UserStoryRequest>;

    public class Validator : Validator<UserStoryRequest>, IValidator {
        public Validator() {
            RuleFor(request => request.Filter)
              .Cascade(CascadeMode.Stop)
              .Must(x => x == null || x.Length >= 3)
              .WithMessage("Title must be null or at least 3 characters long.");
        }
    }
}

public static class ValidationUserWorkStepExtensions {
    public static IServiceCollection AddValidationUserWorkStep(this IServiceCollection services) => services
        .AddScoped<IUserWorkStep<UserStoryRequest, UserStoryResponse>, Validation>()
        .AddScoped<Validation.IValidator, Validation.Validator>();
}



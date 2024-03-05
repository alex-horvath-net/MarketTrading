using Core.Business;
using Core.Solutions.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public class ValidationUserWorkStep(ValidationUserWorkStep.IValidator validator) : UserWorkStep<Request, Response> {
    public async Task<bool> Run(Response response, CancellationToken token) {
        response.MetaData.RequestIssues = await validator.Validate(response.MetaData.Request, token);
        return response.MetaData.RequestIssues.HasFailed();
    }

    public interface IValidator : Core.Business.IValidator<Request>;

    public class Validator : Validator<Request>, ValidationUserWorkStep.IValidator {
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
        .AddScoped<IUserWorkStep<Request, Response>, ValidationUserWorkStep>()
        .AddScoped<ValidationUserWorkStep.IValidator, ValidationUserWorkStep.Validator>();
}



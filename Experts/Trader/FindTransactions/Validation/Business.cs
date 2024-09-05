using Common.Adapters.Validation;
using Common.Adapters.Validation.Model;
using Common.Business.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions.Validation;


public class Business(IValidationTechnologyClient<Request> validator) {
    public async Task<List<Error>> Validate(Request request, CancellationToken token) {
        var technologyModel = await validator.Validate(request, token);
        var businessModel = technologyModel.Select(tm => new Error() { Name = tm.Name, Message = tm.Message }).ToList();
        return businessModel;
    }
}


public class FluentValidatiorClient(FluentValidation.IValidator<Request> technology) : IValidationTechnologyClient<Request> {
    public async Task<List<ErrorTM>> Validate(Request request, CancellationToken token) {
        var technologyData = await technology.ValidateAsync(request, token);
        var technologyModel = technologyData
            .Errors
            .Select(td => new ErrorTM() {
                Name = td.PropertyName,
                Message = td.ErrorMessage
            })
            .ToList();

        return technologyModel;
    }
}


public class FluentValidator : AbstractValidator<Request> {
    public FluentValidator() {
        RuleFor(x => x).NotNull().WithMessage(RequestIsNull);
        RuleFor(x => x.Name).MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(NameIsShort);
    }

    public static string RequestIsNull => "Request must be provided.";
    public static string NameIsShort => "Name must be at least 3 characters long if provided.";
}


public static class Extensions {
    public static IServiceCollection AddValidation(this IServiceCollection services) => services
        .AddScoped<Business>()
            .AddScoped<IValidationTechnologyClient<Request>, FluentValidatiorClient>()
                .AddScoped<IValidator<Request>, FluentValidator>();

}


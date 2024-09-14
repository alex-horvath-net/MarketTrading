using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Validator.FluentValidator;

public class Technology : AbstractValidator<Service.Request> {
    public Technology() {
        RuleFor(x => x).NotNull().WithMessage(RequestIsNull);
        RuleFor(x => x.UserId).NotNull().WithMessage(UserIdIsNull);
        RuleFor(x => x.Name).MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(NameIsShort);
    }

    public static string RequestIsNull => "Request must be provided.";
    public static string UserIdIsNull => "UserId must be provided.";
    public static string NameIsShort => "Name must be at least 3 characters long if it is provided.";
}

public static class TechnologyExtensions {

    public static IServiceCollection AddValidatorTechnology(this IServiceCollection services) => services
        .AddScoped<FluentValidation.IValidator<Service.Request>, Technology>();
}

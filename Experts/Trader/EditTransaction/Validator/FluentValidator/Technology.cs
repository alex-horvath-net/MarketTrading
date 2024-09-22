using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction.Validator.FluentValidator;

public class Technology : AbstractValidator<Service.Request> {
    public Technology(Repository.EntityFramework.Adapter.IClient repository) {
        RuleFor(x => x).NotNull().WithMessage(RequestIsNull);

        RuleFor(x => x.TransactionId).NotNull().WithMessage(TransactionIdNull);
        RuleFor(x => x.TransactionId).MustAsync(repository.ExistsById).WithMessage(TransactionIdNotExists);

        RuleFor(x => x.UserId).NotNull().WithMessage(UserIdIsNull);

        RuleFor(x => x.Name).MinimumLength(3).WithMessage(NameIsShort);
        RuleFor(x => x.Name).MustAsync(repository.NameIsUnique).WithMessage(NameMustBeUniqe);
    }


    public static string RequestIsNull => "Request must be provided.";
    public static string TransactionIdNull => "TransactionId must be provided.";
    public static string TransactionIdNotExists => "Refered TransactionId is not found.";
    public static string UserIdIsNull => "UserId must be provided.";
    public static string NameIsShort => "Name must be at least 3 characters long.";
    public static string NameMustBeUniqe => "Name must be unique.";
}

public static class TechnologyExtensions {

    public static IServiceCollection AddValidatorTechnology(this IServiceCollection services) => services
        .AddScoped<FluentValidation.IValidator<Service.Request>, Technology>();
}

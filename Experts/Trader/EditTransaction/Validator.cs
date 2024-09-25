using Common.Validation.Business.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.EditTransaction;

public class Validator(Validator.IClient validator) : Service.IValidator {
    public async Task<List<Error>> Validate(Service.Request request, CancellationToken token) {
        var adapterIssues = await validator.Validate(request, token);
        var businessIssues = adapterIssues.Select(ToBusiness).ToList();
        return businessIssues;
        static Error ToBusiness(AdapterIssue model) => new(model.Name, model.Message);
    }

    public record AdapterIssue(string Name, string Message);

    public interface IClient {
        Task<List<AdapterIssue>> Validate(Service.Request request, CancellationToken token);
    }

    public class Client(IValidator<Service.Request> validator) : IClient {

        public async Task<List<AdapterIssue>> Validate(Service.Request request, CancellationToken token) {
            var technologyIssues = await validator.ValidateAsync(request, token);
            var adapterIssues = technologyIssues.Errors.Select(ToAdapterIssue).ToList();
            return adapterIssues;
        }

        private static AdapterIssue ToAdapterIssue(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);

        public class Technology : AbstractValidator<Service.Request> {
            public Technology(Repository.IClient repository) {
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
    }
}


public static class ValidatorExtensions {
    public static IServiceCollection AddValidatorAdapter(this IServiceCollection services) => services
        .AddScoped<Service.IValidator, Validator>()
        .AddValidatorClient();

    public static IServiceCollection AddValidatorClient(this IServiceCollection services) => services
        .AddScoped<Validator.IClient, Validator.Client>()
        .AddScoped<Repository.IClient, Repository.Client>()
        .AddValidatorTechnology();

    public static IServiceCollection AddValidatorTechnology(this IServiceCollection services) => services
        .AddScoped<IValidator<Service.Request>, Validator.Client.Technology>();
}
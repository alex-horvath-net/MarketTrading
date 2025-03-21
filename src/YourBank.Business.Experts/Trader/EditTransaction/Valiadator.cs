using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Validation.Business.Model;

namespace Business.Experts.Trader.EditTransaction;

public class Valiadator {
    public class Adapter(Adapter.IInfrastructure infra) : Feature.IValidator {
        public async Task<List<Error>> Validate(Feature.Request request, CancellationToken token) {
            var techModel = await infra.Validate(request, token);
            var businessModel = techModel.Select(model => new Error(model.Name, model.Message)).ToList();
            return businessModel;
        }

        public record TechModel(string Name, string Message);

        public interface IInfrastructure {
            Task<List<TechModel>> Validate(Feature.Request request, CancellationToken token);
        }
    }

    public class Infrastructure(IValidator<Feature.Request> technology) : Adapter.IInfrastructure {
        public async Task<List<Adapter.TechModel>> Validate(Feature.Request request, CancellationToken token) {
            var techData = await technology.ValidateAsync(request, token);
            var techModel = techData.Errors.Select(ToTechModel).ToList();
            return techModel;
        }

        private Adapter.TechModel ToTechModel(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);
    }

    public class Technology : AbstractValidator<Feature.Request> {
        public Technology(Repository.Adapter.IInfrastructure repository) {
            RuleFor(x => x).NotNull().WithMessage("Request must be provided.");

            RuleFor(x => x.TransactionId).NotNull().WithMessage("TransactionId must be provided.");
            RuleFor(x => x.TransactionId).MustAsync(repository.ExistsById).WithMessage("Refered TransactionId is not found.");

            RuleFor(x => x.UserId).NotNull().WithMessage("UserId must be provided.");

            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Name must be at least 3 characters long.");
            RuleFor(x => x.Name).MustAsync(repository.NameIsUnique).WithMessage("Name must be unique.");
        }
    }
}

public static class ValidatorExtensions {
    public static IServiceCollection AddValidatorAdapter(this IServiceCollection services) => services
        .AddScoped<Feature.IValidator, Valiadator.Adapter>()
        .AddScoped<Valiadator.Adapter.IInfrastructure, Valiadator.Infrastructure>()
        //.AddScoped<Repository.IClient, Repository.Client>()
        .AddScoped<IValidator<Feature.Request>, Valiadator.Technology>();
}


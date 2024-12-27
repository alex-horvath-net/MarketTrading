using Common.Validation.Business.Model;
using DomainExperts.Trader.EditTransaction;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DomainExperts.Trader.EditTransaction.WorkSteps;


public class Valiadator {
    public class BusinessAdapter(BusinessAdapter.ITechnologyAdapter technologyAdapter) : BusinessNeed.IValidator {
        public async Task<List<Error>> Validate(BusinessNeed.Request request, CancellationToken token) {
            var techModel = await technologyAdapter.Validate(request, token);
            var businessModel = techModel.Select(model => new Error(model.Name, model.Message)).ToList();
            return businessModel;
        }

        public record TechModel(string Name, string Message);

        public interface ITechnologyAdapter {
            Task<List<TechModel>> Validate(BusinessNeed.Request request, CancellationToken token);
        }
    }

    public class TechnologyAdapter(IValidator<BusinessNeed.Request> technology) : BusinessAdapter.ITechnologyAdapter {
        public async Task<List<BusinessAdapter.TechModel>> Validate(BusinessNeed.Request request, CancellationToken token) {
            var techData = await technology.ValidateAsync(request, token);
            var techModel = techData.Errors.Select(ToTechModel).ToList();
            return techModel;
        }

        private BusinessAdapter.TechModel ToTechModel(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);
    }

    public class Technology : AbstractValidator<BusinessNeed.Request> {
        public Technology(Repository.BusinessAdapter.ITechnologyAdapter repository) {
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
        .AddScoped<BusinessNeed.IValidator, Valiadator.BusinessAdapter>()
        .AddScoped<Valiadator.BusinessAdapter.ITechnologyAdapter, Valiadator.TechnologyAdapter>()
        //.AddScoped<Repository.IClient, Repository.Client>()
        .AddScoped<IValidator<BusinessNeed.Request>, Valiadator.Technology>();
}


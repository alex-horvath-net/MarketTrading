using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.EditTransaction;

public class Validate {
    public class Business(Business.IAdapter adapter) : Feature.IValidate {
        public async Task<bool> Run(Feature.Response response) {
            response.Errors = await adapter.Validate(response.Request, response.Token);
            return response.Errors.Any();
        }

        public interface IAdapter {
            Task<List<Domain.Error>> Validate(Feature.Request request, CancellationToken token);
        }
    }

    public class Adapter(Adapter.IInfrastructure infrastructure) : Business.IAdapter {
        public async Task<List<Domain.Error>> Validate(Feature.Request request, CancellationToken token) {
            var infraModel = await infrastructure.Validate(request, token);
            var domainModel = infraModel.Select(model => new Domain.Error(model.Name, model.Message)).ToList();
            return domainModel;
        }

        public interface IInfrastructure {
            Task<List<Adapter.InfraModel>> Validate(Feature.Request request, CancellationToken token);
        }

        public record InfraModel(string Name, string Message);
    }

    public class Infrastructure : FluentValidation.AbstractValidator<Feature.Request>, Adapter.IInfrastructure {
        public Infrastructure(Edit.Adapter.IInfrastructure editInfra) {
            RuleFor(x => x).NotNull().WithMessage("Request must be provided.");

            RuleFor(x => x.TransactionId).NotNull().WithMessage("TransactionId must be provided.");
            RuleFor(x => x.TransactionId).MustAsync(async (id, token) => {
                var transaction = await editInfra.FindById(id, token);
                return transaction != null;
            }).WithMessage("Refered TransactionId is not found.");

            RuleFor(x => x.UserId).NotNull().WithMessage("UserId must be provided.");

            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Name must be at least 3 characters long.");
            RuleFor(x => x.Name).MustAsync(async (name, token) => {
                var transaction = await editInfra.FindByName(name, token);
                return transaction == null;
            }).WithMessage("Name must be unique.");
        }

        public async Task<List<Adapter.InfraModel>> Validate(Feature.Request request, CancellationToken token) {
            var infra = await ValidateAsync(request, token);
            var infraModel = infra.Errors
                .Where(x => x.Severity == Severity.Error)
                .Select(x => new Adapter.InfraModel(x.PropertyName, x.ErrorMessage))
                .ToList();

            return infraModel;
        }
    }
}

public static class ValidateExtensions {
    public static IServiceCollection AddValidate(this IServiceCollection services) => services
        .AddScoped<Feature.IValidate, Validate.Business>()
        .AddScoped<Validate.Business.IAdapter, Validate.Adapter>()
        .AddScoped<Validate.Adapter.IInfrastructure, Validate.Infrastructure>();
}
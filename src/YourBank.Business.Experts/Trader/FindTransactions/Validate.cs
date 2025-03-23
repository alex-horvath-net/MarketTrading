using Business.Domain;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Experts.Trader.FindTransactions;

public class Validate {
    public class Business(Business.IAdapter adapter) : Feature.IValidate {
        public async Task<bool> Run(Feature.Response response) {
            response.Errors = await adapter.Validate(response.Request, response.Token);
            return response.Errors.Any();
        }

        public interface IAdapter {
            Task<List<Error>> Validate(Feature.Request request, CancellationToken token);
        }
    }

    public class Adapter(Adapter.IInfrastructure infrastructure) : Business.IAdapter {
        public async Task<List<Error>> Validate(Feature.Request request, CancellationToken token) {
            var infraModel = await infrastructure.Validate(request, token);
            var domainModel = infraModel.Select(model => new Error(model.Name, model.Message)).ToList();
            return domainModel;
        }

        public interface IInfrastructure {
            Task<List<Adapter.InfraModel>> Validate(Feature.Request request, CancellationToken token);
        }

        public record InfraModel(string Name, string Message);
    }

    public class Infrastructure : FluentValidation.AbstractValidator<Feature.Request>, Adapter.IInfrastructure {
        public Infrastructure() {
            RuleFor(x => x).NotNull().WithMessage("Request must be provided.");
            RuleFor(x => x.UserId).NotNull().WithMessage("UserId must be provided.");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Name must be at least 3 characters long.");
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
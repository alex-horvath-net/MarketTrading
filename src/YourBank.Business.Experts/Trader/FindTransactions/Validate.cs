using FluentValidation;
using Infrastructure.Validation.Business.Model;
using Microsoft.Extensions.DependencyInjection;
//using Senitizer.Validate.Business.Model;

namespace Business.Experts.Trader.FindTransactions;

public class Validate {
    public class Business(Business.IAdapter adapter) : Feature.IValidate {
        public async Task<bool> Run(Feature.Response response) {
            var adapterModel = await adapter.Validate(response.Request, response.Token);
            var domainModel = adapterModel.Select(model => new Error(model.Name, model.Message)).ToList();
            response.Errors = domainModel;
            return response.Errors.Any(); 
        }
         
        public record AdapterModel(string Name, string Message);

        public interface IAdapter {
            Task<List<AdapterModel>> Validate(Feature.Request request, CancellationToken token);
        }
    }

    public class Adapter(IValidator<Feature.Request> infrastructure) : Business.IAdapter {
        public async Task<List<Business.AdapterModel>> Validate(Feature.Request request, CancellationToken token) {
            var infraModel = await infrastructure.ValidateAsync(request, token);
            var adapterModel = infraModel.Errors.Select(Model).ToList();
            return adapterModel;
        } 

        private Business.AdapterModel Model(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);
    }

    public class Infrastructure : FluentValidation.AbstractValidator<Feature.Request> {
        public Infrastructure() {
            RuleFor(x => x).NotNull().WithMessage("Request must be provided.");
            RuleFor(x => x.UserId).NotNull().WithMessage("UserId must be provided.");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Name must be at least 3 characters long.");
        }
    }
}

public static class ValidatorExtensions {
    public static IServiceCollection AddValidatorAdapter(this IServiceCollection services) => services
        .AddScoped<Feature.IValidate, Validate.Business>()
        .AddScoped<Validate.Business.IAdapter, Validate.Adapter>()
        //.AddScoped<Repository.ISanitizer, Repository.Senitizer>()
        .AddScoped<IValidator<Feature.Request>, Validate.Infrastructure>();
}
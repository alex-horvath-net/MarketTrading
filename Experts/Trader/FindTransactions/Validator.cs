using Common.Validation.Business.Model;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public class Validator(Validator.IClient client) : Service.IValidator {
    public async Task<List<Error>> Validate(Service.Request request, CancellationToken token) {
        var clientModel = await client.Validate(request, token);
        var businessModel = clientModel.Select(ToBusiness).ToList();
        token.ThrowIfCancellationRequested();
        return businessModel;
        static Error ToBusiness(ClientModel model) => new(model.Name, model.Message);
    }

    public record ClientModel(string Name, string Message);

    public interface IClient {
        Task<List<ClientModel>> Validate(Service.Request request, CancellationToken token);
    }

    public class Client(IValidator<Service.Request> technology) : IClient {
        public async Task<List<ClientModel>> Validate(Service.Request request, CancellationToken token) {
            var techModel = await technology.ValidateAsync(request, token);
            var clientModel = techModel.Errors.Select(ToModel).ToList();
            token.ThrowIfCancellationRequested();
            return clientModel;
        }

        private static ClientModel ToModel(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);

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

    }
}

public static class AdapterExtensions {

    public static IServiceCollection AddValidatorAdapter(this IServiceCollection services) => services
        .AddScoped<Service.IValidator, Validator>()
        .AddValidatorClient();

    public static IServiceCollection AddValidatorClient(this IServiceCollection services) => services
       .AddScoped<Validator.IClient, Validator.Client>()
       .AddValidatorTechnology();

    public static IServiceCollection AddValidatorTechnology(this IServiceCollection services) => services
       .AddScoped<IValidator<Service.Request>, Validator.Client.Technology>();
}


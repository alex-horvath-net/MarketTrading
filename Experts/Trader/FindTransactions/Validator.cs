using DomainExperts.Trader.FindTransactions.Feature;
using FluentValidation;
using Infrastructure.Validation.Business.Model;

namespace DomainExperts.Trader.FindTransactions;

public class Validator(Validator.IClient client) : Feature.OutputPorts.IValidator {
    public async Task<List<Error>> Validate(Request request, CancellationToken token) {
        var clientModel = await client.Validate(request, token);
        var businessModel = clientModel.Select(ToBusiness).ToList();
        token.ThrowIfCancellationRequested();
        return businessModel;
        static Error ToBusiness(ClientModel model) => new(model.Name, model.Message);
    }

    public record ClientModel(string Name, string Message);

    public interface IClient {
        Task<List<ClientModel>> Validate(Request request, CancellationToken token);
    }

    public class Client(IValidator<Request> technology) : IClient {
        public async Task<List<ClientModel>> Validate(Request request, CancellationToken token) {
            var techModel = await technology.ValidateAsync(request, token);
            var clientModel = techModel.Errors.Select(ToModel).ToList();
            token.ThrowIfCancellationRequested();
            return clientModel;
        }

        private static ClientModel ToModel(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);

        public class Technology : AbstractValidator<Request> {
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

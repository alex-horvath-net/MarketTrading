using System.ComponentModel;
using Common.Adapters.App.Data.Model;
using Common.Adapters.Blazor;
using Common.Business.Model;
using Common.Technology.EF.App;
using Common.Validation.Business.Model;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;

public class Triggers {
    public class Blazor {
        public interface ITrigger {
            Task<ViewModel> Execute(string name, string userId, CancellationToken token);
        }

        public record ViewModel {
            public MetaVM Meta { get; set; }
            public List<ErrorVM> Errors { get; set; } = [];
            public DataListModel<TransactionVM> Transactions { get; set; }
            public class MetaVM {
                public Guid Id { get; internal set; }
            }

            public class ErrorVM {
                public string Name { get; internal set; }
                public string Message { get; internal set; }
            }

            public record TransactionVM {
                [DisplayName("ID")]
                public long Id { get; set; }
                public string Name { get; set; }
            }
        }

        public class Trigger(UserStory.IUserStory service) : ITrigger {
            public async Task<ViewModel> Execute(string name, string userId, CancellationToken token) {
                var request = new UserStory.Request {
                    Name = name,
                    UserId = userId
                };

                token.ThrowIfCancellationRequested();

                var response = await service.Execute(request, token);

                var viewModel = new ViewModel();

                viewModel.Meta = ToMetaViewModel(response.Request);
                viewModel.Errors = response.Errors.Select(ToErrorViewModel).ToList();
                viewModel.Transactions = new();
                viewModel.Transactions.Rows = response.Transactions.Select(ToTranaztionViewModel).ToList();
                viewModel.Transactions.Columns.Add(x => x.Id);
                viewModel.Transactions.Columns.Add(x => x.Name);

                token.ThrowIfCancellationRequested();

                return viewModel;

                static ViewModel.MetaVM ToMetaViewModel(UserStory.Request businessModel) => new() {
                    Id = businessModel.Id,
                };

                static ViewModel.TransactionVM ToTranaztionViewModel(Transaction businessModel) => new() {
                    Id = businessModel.Id,
                    Name = businessModel.Name
                };

                static ViewModel.ErrorVM ToErrorViewModel(Error businessModel) => new() {
                    Name = businessModel.Name,
                    Message = businessModel.Message
                };
            }
        }
    }
}

public class UserStory {
    public interface IUserStory {
        Task<Response> Execute(Request request, CancellationToken token);
    }
    public class Request {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string UserId { get; set; }
    }

    public class Response {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsUnderConstruction { get; set; } = false;
        public DateTime? StopedAt { get; set; }
        public DateTime? FailedAt { get; internal set; }
        public Exception? Exception { get; set; }
        public Request? Request { get; set; }
        public List<Error> Errors { get; set; } = [];
        public List<Transaction> Transactions { get; set; } = [];
    }

    public class WorkFlow(WorkFlow.IValidator validator, WorkFlow.IFlag flag, WorkFlow.IRepository repository, WorkFlow.IClock clock) : IUserStory {

        public async Task<Response> Execute(Request request, CancellationToken token) {
            var response = new Response();
            try {
                response.IsUnderConstruction = flag.IsPublic(request, token);
                response.Request = request;
                response.Errors = await validator.Validate(request, token);
                if (response.Errors.Count > 0)
                    return response;


                response.Transactions = await repository.FindTransactions(request, token);

                token.ThrowIfCancellationRequested();
            } catch (Exception ex) {
                response.Exception = ex;
            } finally {
                response.StopedAt = clock.GetTime();
            }
            return response;
        }

        public interface IFlag { bool IsPublic(Request request, CancellationToken token); }

        public interface IClock { DateTime GetTime(); }

        public interface IValidator { Task<List<Error>> Validate(Request request, CancellationToken token); }

        public interface IRepository { Task<List<Transaction>> FindTransactions(Request request, CancellationToken token); }
    }
}

public class WorkSteps {
    public class Clock(Clock.IClient client) : UserStory.WorkFlow.IClock {
        public DateTime GetTime() => client.Now;

        public interface IClient { DateTime Now { get; } }

        public class Client : IClient {
            public DateTime Now => DateTime.Now;
        }
    }

    public class Flag(Flag.IClient client) : UserStory.WorkFlow.IFlag {
        public bool IsPublic(UserStory.Request request, CancellationToken token) {
            var isPublic = client.IsEnabled();
            token.ThrowIfCancellationRequested();
            return isPublic;
        }
        public interface IClient {
            bool IsEnabled();
        }

        public class Client : IClient {
            public bool IsEnabled() => false;
        }
    }

    public class Repository(Repository.IClient client) : UserStory.WorkFlow.IRepository {
        public async Task<List<Transaction>> FindTransactions(UserStory.Request request, CancellationToken token) {
            var dataModel = await client.Find(request.Name, token);
            var businessModel = dataModel.Select(ToBusinessModel).ToList();

            token.ThrowIfCancellationRequested();
            return businessModel;
        }

        private static List<Transaction> ToBusinessModelList(List<TransactionDM> dataModelList) => dataModelList.Select(ToBusinessModel).ToList();
        private static Transaction ToBusinessModel(TransactionDM dataModel) => new() {
            Id = dataModel.Id,
            Name = dataModel.Name
        };


        public interface IClient {
            public Task<List<TransactionDM>> Find(string? name, CancellationToken token);
        }
        public class Client(AppDB db) : IClient {
            public async Task<List<TransactionDM>> Find(string? name, CancellationToken token) {
                token.ThrowIfCancellationRequested();

                var transactions = name == null ?
                   await db.Transactions.AsNoTracking().ToListAsync(token) :
                   await db.Transactions.AsNoTracking().Where(x => x.Name.Contains(name)).ToListAsync(token);

                token.ThrowIfCancellationRequested();

                return transactions;
            }
        }

    }

    public class Validator(Validator.IClient client) : UserStory.WorkFlow.IValidator {
        public async Task<List<Error>> Validate(UserStory.Request request, CancellationToken token) {
            var clientModel = await client.Validate(request, token);
            var businessModel = clientModel.Select(ToBusiness).ToList();
            token.ThrowIfCancellationRequested();
            return businessModel;
            static Error ToBusiness(ClientModel model) => new(model.Name, model.Message);
        }

        public record ClientModel(string Name, string Message);

        public interface IClient {
            Task<List<ClientModel>> Validate(UserStory.Request request, CancellationToken token);
        }

        public class Client(IValidator<UserStory.Request> technology) : IClient {
            public async Task<List<ClientModel>> Validate(UserStory.Request request, CancellationToken token) {
                var techModel = await technology.ValidateAsync(request, token);
                var clientModel = techModel.Errors.Select(ToModel).ToList();
                token.ThrowIfCancellationRequested();
                return clientModel;
            }

            private static ClientModel ToModel(FluentValidation.Results.ValidationFailure tech) => new(tech.PropertyName, tech.ErrorMessage);

            public class Technology : AbstractValidator<UserStory.Request> {
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

}


public static class FindTransactionsExtensions {
    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<Triggers.Blazor.ITrigger, Triggers.Blazor.Trigger>()
        .AddService(configuration);

    public static IServiceCollection AddService(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<UserStory.IUserStory, UserStory.WorkFlow>()
        .AddClock()
        .AddFlag()
        .AddValidator()
        .AddRepository();

    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<UserStory.WorkFlow.IClock, WorkSteps.Clock>()
        .AddScoped<WorkSteps.Clock.IClient, WorkSteps.Clock.Client>();

    public static IServiceCollection AddFlag(this IServiceCollection services) => services
        .AddScoped<UserStory.WorkFlow.IFlag, WorkSteps.Flag>()
        .AddScoped<WorkSteps.Flag.IClient, WorkSteps.Flag.Client>();

    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<UserStory.WorkFlow.IRepository, WorkSteps.Repository>()
        .AddScoped<WorkSteps.Repository.IClient, WorkSteps.Repository.Client>();


    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<UserStory.WorkFlow.IValidator, WorkSteps.Validator>()
        .AddScoped<WorkSteps.Validator.IClient, WorkSteps.Validator.Client>()
        .AddScoped<IValidator<UserStory.Request>, WorkSteps.Validator.Client.Technology>();
}




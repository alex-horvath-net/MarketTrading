using Experts.Trader.FindTransactions.Triggers.Blazor;
using Experts.Trader.FindTransactions.Triggers.Blazor.InputPort;
using Experts.Trader.FindTransactions.UserStory;
using Experts.Trader.FindTransactions.UserStory.InputPort;
using Experts.Trader.FindTransactions.UserStory.OutputPort;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Trader.FindTransactions;


public static class FindTransactionsExtensions {
    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<ITrigger, Trigger>()
        .AddService(configuration);

    public static IServiceCollection AddService(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IUserStory, WorkFlow>()
        .AddClock()
        .AddFlag()
        .AddValidator()
        .AddRepository();

    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<IClock, WorkSteps.Clock>()
        .AddScoped<WorkSteps.Clock.IClient, WorkSteps.Clock.Client>();

    public static IServiceCollection AddFlag(this IServiceCollection services) => services
        .AddScoped<IFlag, WorkSteps.Flag>()
        .AddScoped<WorkSteps.Flag.IClient, WorkSteps.Flag.Client>();

    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepository, WorkSteps.Repository>()
        .AddScoped<WorkSteps.Repository.IClient, WorkSteps.Repository.Client>();


    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<UserStory.OutputPort.IValidator, WorkSteps.Validator>()
        .AddScoped<WorkSteps.Validator.IClient, WorkSteps.Validator.Client>()
        .AddScoped<IValidator<Request>, WorkSteps.Validator.Client.Technology>();
}




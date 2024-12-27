using DomainExperts.Trader.FindTransactions.Triggers.Blazor;
using DomainExperts.Trader.FindTransactions.Triggers.Blazor.InputPort;
using DomainExperts.Trader.FindTransactions.UserStory;
using DomainExperts.Trader.FindTransactions.UserStory.InputPort;
using DomainExperts.Trader.FindTransactions.UserStory.OutputPort;
using DomainExperts.Trader.FindTransactions.WorkSteps;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainExperts.Trader.FindTransactions;


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
        .AddScoped<IClock, Clock>()
        .AddScoped<Clock.IClient, Clock.Client>();

    public static IServiceCollection AddFlag(this IServiceCollection services) => services
        .AddScoped<IFlag, Flag>()
        .AddScoped<Flag.IClient, Flag.Client>();

    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepository, Repository>()
        .AddScoped<Repository.IClient, Repository.Client>();


    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<UserStory.OutputPort.IValidator, Validator>()
        .AddScoped<Validator.IClient, Validator.Client>()
        .AddScoped<IValidator<Request>, Validator.Client.Technology>();
}




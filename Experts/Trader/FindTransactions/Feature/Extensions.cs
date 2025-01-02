using DomainExperts.Trader.FindTransactions.Clock;
using DomainExperts.Trader.FindTransactions.Feature.OutputPorts;
using DomainExperts.Trader.FindTransactions.Triggers.Blazor;
using DomainExperts.Trader.FindTransactions.Triggers.Blazor.InputPort;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainExperts.Trader.FindTransactions.Feature;

public static class Extensions {
    public static IServiceCollection AddFindTransactions(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<ITrigger, Trigger>()
        .AddService(configuration);

    public static IServiceCollection AddService(this IServiceCollection services, ConfigurationManager configuration) => services
        .AddScoped<IFeature, Featrure>()
        .AddClock()
        .AddFlag()
        .AddValidator()
        .AddRepository();

    public static IServiceCollection AddClock(this IServiceCollection services) => services
        .AddScoped<IClockAdapter, DefaultClockAdapter>()
        .AddScoped<IClock, DefaultClock>();

    public static IServiceCollection AddFlag(this IServiceCollection services) => services
        .AddScoped<IFlag, Flag>()
        .AddScoped<Flag.IClient, Flag.Client>();

    public static IServiceCollection AddRepository(this IServiceCollection services) => services
        .AddScoped<IRepository, Repository>()
        .AddScoped<Repository.IClient, Repository.Client>();


    public static IServiceCollection AddValidator(this IServiceCollection services) => services
        .AddScoped<OutputPorts.IValidator, Validator>()
        .AddScoped<Validator.IClient, Validator.Client>()
        .AddScoped<IValidator<Request>, Validator.Client.Technology>();
}


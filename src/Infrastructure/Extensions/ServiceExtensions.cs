using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Validation.FluentValidator.Adapters;
using Infrastructure.Validation.FluentValidator.Technology;
using Infrastructure.Adapters.Identity.Data.Model;
using Infrastructure.Technology.EF.Identity;
using Infrastructure.Technology.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Extensions;
public static class ServiceExtensions {

    public static IServiceCollection AddValidatorClient<TRequest>(this IServiceCollection services) => services
        .AddScoped<ICommonClient<TRequest>, CommonClient<TRequest>>();

  

}

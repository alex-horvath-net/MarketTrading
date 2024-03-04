using Core.Business;
using Core.Business.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Solutions.Validation;
public static class Extensions {
  public static IServiceCollection AddFluentValidation(this IServiceCollection services) {
    services.AddScoped(typeof(IValidator<>), typeof(Validator<>));
    return services;
  }

  public static bool HasFailed(this IEnumerable<Result> results) => results.Any(x => x.IsFailed);
}
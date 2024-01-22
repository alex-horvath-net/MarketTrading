using Common.Business;
using Common.Solutions.Data.MainDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class Extensions {
    public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration) => services
     .AddCommonBusiness()
     .AddCommonSolutions(configuration);
}

using Blogger.ReadPosts.Tasks.DataAccessUnit;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Adapters.DataAccessUnit;

public static class Extensions
{
    public static IServiceCollection AddDataAccessAdapter(this IServiceCollection services)
    {
        services.AddScoped<IDataAccessAdapter, DataAccessAdapter>();
        return services;
    }
}                                                                             

using Blogger.ReadPosts.Tasks.ValidationUnit;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.ReadPosts.Adapters.ValidationUnit;

public static class Extensions
{
    public static IServiceCollection AddValidationAdapter(this IServiceCollection services)
    {
        services.AddScoped<IValidationAdapter, ValidationAdapter>();
        return services;
    }
}

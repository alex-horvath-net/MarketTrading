using Experts.Blogger;
using Microsoft.Extensions.DependencyInjection;

namespace Experts;

public static class Extensions {
    public static IServiceCollection AddExperts(this IServiceCollection services) => services
        .AddBlogger()
        ;

}

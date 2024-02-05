
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public static class Extensions {
  public static IServiceCollection AddReadPosts(this IServiceCollection services) => services
    .AddScoped<Business.IUserStory, Business.UserStory>()
    .AddScoped<Business.IValidator, Solutions.Validation>()
    .AddScoped<Business.IRepository, Solutions.Repository>();
}


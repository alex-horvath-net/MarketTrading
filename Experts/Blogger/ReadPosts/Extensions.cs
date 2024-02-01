
using Core.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public static class Extensions {
  public static IServiceCollection AddReadPosts(this IServiceCollection services) => services
    .AddScoped<IStory<Request, Response, Story>, Story>()
    .AddScoped<IValidator, Validation>()
    .AddScoped<IRepository, Repository>();
}

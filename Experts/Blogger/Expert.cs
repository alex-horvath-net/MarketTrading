

using Experts.Blogger.ReadPosts;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger;

public record Expert(
  ReadPosts.Business.IUserStory ReadPosts,
  ReadPosts.Business.IUserStory GetPost);


public static class Extensions {
  public static IServiceCollection AddBlogger(this IServiceCollection services) => services

      .AddScoped<Expert>()
      .AddReadPosts();

}
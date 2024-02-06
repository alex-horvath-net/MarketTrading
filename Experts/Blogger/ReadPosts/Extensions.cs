using Core.Solutions.Setting;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public static class Extensions {
  public static IServiceCollection AddReadPosts(this IServiceCollection services) => services
    .AddSettings<Business.Model.Settings>()
    .AddScoped<Business.IUserStory, Business.UserStory>()
    .AddScoped<Business.IValidator, Solutions.Validation>()
    .AddScoped<Business.IRepository, Solutions.Repository>();


    
}


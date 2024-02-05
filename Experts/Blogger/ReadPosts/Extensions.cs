
using Experts.Blogger.ReadPosts.Business.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Experts.Blogger.ReadPosts;

public static class Extensions {
  public static IServiceCollection AddReadPosts(this IServiceCollection services, Action<Business.Model.Settings> optionBuilder = null) => services
    .AddOptionsWithValidateOnStart<Business.Model.Settings>()
      .Configure<IConfiguration>((options, config) => {
        config.GetSection(Settings.SectionName).Bind(options);
        optionBuilder ??= _ => { };
        optionBuilder(options);
      })
    .ValidateDataAnnotations()
    .Services
    .AddScoped<Business.IUserStory, Business.UserStory>()
    .AddScoped<Business.IValidator, Solutions.Validation>()
    .AddScoped<Business.IRepository, Solutions.Repository>();
}


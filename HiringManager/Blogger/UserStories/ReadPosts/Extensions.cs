using Core.Application;
using Core.Enterprise;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ReadTask.Sockets.DataAccessSocket;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask;
using Users.Blogger.UserStories.ReadPosts.UserTasks.ValidationTask.Sockets.ValidationSocket;
using Xunit;

namespace Users.Blogger.UserStories.ReadPosts;

public static class Extensions
{
    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services)
    {
        services.AddReadTask();
        services.AddValidationTask();

        return services;
    }

    public class Design
    {
        [Fact]
        public async Task AddReadPostsUserStory()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services
                .AddCore()
                .AddCommon(configuration)
                .AddReadPostsUserStory();

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetRequiredService<IValidationPlugin>();
            serviceProvider.GetRequiredService<IDataAccessPlugin>();

            serviceProvider.GetRequiredService<IValidationSocket>();
            serviceProvider.GetRequiredService<IDataAccessPlugin>();

            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IWorkStep<Response>>();
            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IFeature<Request, Response>>();
        }
    }
}

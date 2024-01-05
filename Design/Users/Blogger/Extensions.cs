using Design.Users.Blogger.ReadPostsUserStory;
using Design.Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;
using Design.Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory;
using Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;
using Users.Blogger.ReadPostsUserStory.ValidationTask;
using Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket;

namespace Design.Users.Blogger;

public static class Extensions
{
    public static IServiceCollection AddBlogger(this IServiceCollection services)
    {
        services.AddReadPostsUserStory();

        return services;
    }

    public class Design
    {
        [Fact]
        public async Task AddBlogger()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services
                .AddCore()
                .AddCommon(configuration)
                .AddBlogger();

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetRequiredService<ValidationSocket.IValidationPlugin>();
            serviceProvider.GetRequiredService<IDataAccessPlugin>();

            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IWorkStep<Response>>();
            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IFeature<Request, Response>>();
        }
    }
}

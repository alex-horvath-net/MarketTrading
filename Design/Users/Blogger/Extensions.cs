using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory;
using Core.Application;
using Core.Enterprise;
using Users.Blogger.ReadPostsUserStory.ReadUserTask.DataAccessSocket;

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

            serviceProvider.GetRequiredService<global::Users.Blogger.ReadPostsUserStory.ValidationUserTask.ValidationSocket.Socket.IValidationPlugin>();
            serviceProvider.GetRequiredService<IDataAccessPlugin>();

            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IWorkStep<Response>>();
            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IFeature<Request, Response>>();
        }
    }
}

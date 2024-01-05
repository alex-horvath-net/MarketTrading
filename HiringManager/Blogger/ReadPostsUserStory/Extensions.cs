using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Blogger.ReadPostsUserStory.ReadTask;
using Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;
using Users.Blogger.ReadPostsUserStory.ValidationTask;
using Users.Blogger.ReadPostsUserStory.ValidationTask.ValidationSocket;

namespace Users.Blogger.ReadPostsUserStory;

public static class Extensions
{
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

            serviceProvider.GetRequiredService<ValidationSocket.IValidationPlugin>();
            serviceProvider.GetRequiredService<DataAccessSocket.IDataAccessPlugin>();

            serviceProvider.GetRequiredService<IValidationSocket>();
            serviceProvider.GetRequiredService<DataAccessSocket.IDataAccessPlugin>();

            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IWorkStep<Response>>();
            //serviceProvider.GetRequiredService<Core.Enterprise.BusinessWorkFlow.IFeature<Request, Response>>();
        }
    }

    public static IServiceCollection AddReadPostsUserStory(this IServiceCollection services)
    {
        services.AddReadTask();
        services.AddValidationTask();

        return services;
    }
}

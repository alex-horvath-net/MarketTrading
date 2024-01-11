using BusinessExperts.Blogger.ReadPostsExpertStory;
using Common;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Design.BusinessExperts.Blogger;

public static class Extensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) {
        services.AddReadPostsUserStory();

        return services;
    }

    public class Design {
        [Fact]
        public async Task AddBlogger() {
            var configurationBuilder = new ConfigurationBuilder();
            var configuration = configurationBuilder.Build();
            var env = Environments.Development;
            var services = new ServiceCollection();

            services
                .AddCoreSystem()
                .AddCoreApplication(configuration, isDevelopment: true)
                .AddBlogger();

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetRequiredService<IValidationPlugin>();
            serviceProvider.GetRequiredService<IReadPlugin>();

            //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IWorkStep<Response>>();
            //serviceProvider.GetRequiredService<Core.BusinessWorkFlow.IFeature<Request, Response>>();
        }
    }
}

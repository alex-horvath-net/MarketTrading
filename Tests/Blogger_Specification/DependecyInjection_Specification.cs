using Blogger;
using Core.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Marker = Blogger.ReadPosts.Business.IFeature;

namespace Specifications.Blogger_Specification;

public class DependecyInjection_Specification
{
    [Fact]
    public async void Inject_AddBlogger_Dependecies()
    {                                                                    
        var configurationBuilder = new ConfigurationBuilder();
        var configuration = configurationBuilder.Build();

        var unit = new ServiceCollection();

        var services = unit.AddCore(configuration).AddBlogger();
        using var serviceProvider = services.BuildServiceProvider();
        var markerService = serviceProvider.GetRequiredService<Marker>();

        services.Should().NotBeNull();
    }
}

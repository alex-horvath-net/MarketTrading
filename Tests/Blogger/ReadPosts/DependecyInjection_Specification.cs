using Blogger.ReadPosts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Blogger.ReadPosts;

public class DependecyInjection_Specification
{
    [Fact]
    public async void Inject_AddReadPosts_Dependecies()
    {
        var unit = new ServiceCollection();

        var services = unit.AddReadPosts();

        services.Should().NotBeNull();
    }   
}
                                               
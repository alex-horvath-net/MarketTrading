using Blogger;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Blogger;

public class DependecyInjection_Specification
{
    [Fact]
    public async void Inject_AddReadPosts_Dependecies()
    {
        var unit = new ServiceCollection();

        var services = unit.AddBlogger();

        services.Should().NotBeNull();
    }   
}

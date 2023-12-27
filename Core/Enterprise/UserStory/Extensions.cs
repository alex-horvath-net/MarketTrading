using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Enterprise.UserStory;

public static class Extensions
{
    public static IServiceCollection AddUserStory(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserStory<,>), typeof(UserStoryCore<,>));

        return services;
    }
}

public class Extensions_Design
{
    [Fact]
    public void AddUserStory_Registers_All_UserStory()
    {
        var services = new ServiceCollection();

        services.AddUserStory();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserStory<RequestCore, ResponseCore<RequestCore>>>();
        userStory.Should().NotBeNull();
    }
}


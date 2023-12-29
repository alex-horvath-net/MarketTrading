using System.Runtime.CompilerServices;
using Core.Enterprise.UserStory;
using Core.Enterprise.UserTasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Enterprise;



public class Extensions_Design
{
    [Fact]
    public void AddUserStory_Registers_All_UserStory()
    {
        var services = new ServiceCollection();

        services.AddCore();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserStory<RequestCore, ResponseCore<RequestCore>>>();
        userStory.Should().NotBeNull();
    }
}

public static class Extensions
{

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}

using Core.Enterprise.UserStory;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Enterprise.UserTasks;

public static class Extensions
{
    public static IServiceCollection AddFeatureTask(this IServiceCollection services)
    {
        services.AddScoped(typeof(IUserTask<,>), typeof(FeatureTask<,>));

        return services;
    }
}


public class Extensions_Design
{
    [Fact]
    public void AddFeatureTask_Registers_All_UserTask()
    {
        var services = new ServiceCollection();

        services.AddFeatureTask();

        var sp = services.BuildServiceProvider();
        var userStory = sp.GetRequiredService<IUserTask<RequestCore, ResponseCore<RequestCore>>>();
        userStory.Should().NotBeNull();
    }
}


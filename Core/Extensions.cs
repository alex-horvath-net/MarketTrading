using Core.UserStory;
using Core.UserTasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core;

public static class Extensions
{
    public static Task<T> ToTask<T>(this T value) => Task.FromResult(value);

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}


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

    [Fact]
    public void ToTask_Conver_To_Task()
    {
        var year = 1984;

        var yearTask = year.ToTask();

        yearTask.Should().NotBeNull();
        yearTask.Should().BeOfType<Task<int>>();
        yearTask.Result.Should().Be(year);

    }
}
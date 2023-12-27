using System.Runtime.CompilerServices;
using Core.Enterprise.UserStory;
using Core.Enterprise.UserTasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Core.Enterprise;

public static class Extensions
{
    public async static void FireAndForget(this Task task) =>
        task.FireAndForget<Exception>(returnToCallerTread: false, handleException: ex => { }, retrhrow: true);

    public async static void FireAndForget<TException>(this Task task, bool returnToCallerTread, Action<TException> handleException, bool retrhrow) where TException : Exception
    {
        try
        {
            await task.ConfigureAwait(returnToCallerTread);
        } catch (TException ex)
        {
            handleException(ex);

            if (retrhrow)
                throw;
        }
    }


    public static async IAsyncEnumerable<TResult> Yield<TResult, TFrom>(this IEnumerable<TFrom> list,
        Func<TFrom, CancellationToken, Task<TResult>> factory,
        [EnumeratorCancellation] CancellationToken token)
    {
        var tasks = list.Select(item => factory(item, token)).ToList();
        while (tasks.Count > 0)
        {
            var completedTask = await Task.WhenAny(tasks).ConfigureAwait(false);
            tasks.Remove(completedTask);
            yield return await completedTask.ConfigureAwait(false);
        }
    }


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
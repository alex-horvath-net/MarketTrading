using System.Diagnostics;
using System.Text;
using Core.Enterprise.UserStory;
using Core.Enterprise.UserTasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Core.Enterprise;

public static class Extensions
{
    public static T DumpOld<T>(this T instance, ITestOutputHelper output, string? message = null)
    {
        var sb = new StringBuilder();

        sb.Append($"{DateTime.UtcNow:dd MMM HH:mm:ss ffff} ");

        if (instance is Task<T> taskT)
        { 
            sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3}-{taskT.Id:D3} ");
            sb.Append($"Status: {taskT.Status} ");
            sb.Append($"Result: {taskT.Result} ");
        } else if (instance is Task task)
        {
            sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3}-{task.Id:D3} ");
            sb.Append($"Status: {task.Status} ");
        } else
        {
            sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3}-000 ");
        }

        if (message != null)
        {
            sb.Append($"Message: {message}");
        }

        output.WriteLine(sb.ToString());
        Debug.WriteLine(sb.ToString());

        return instance;
    }

    public static Task<T> Dump<T>(this Task<T> taskT, ITestOutputHelper output, string? message = null)
    {
        var sb = new StringBuilder();

        sb.Append($"{DateTime.UtcNow:dd MMM HH:mm:ss ffff} ");
        sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3}-{taskT.Id:D3} ");
        sb.Append($"Status: {taskT.Status} ");
        //sb.Append($"Result: {taskT.Result} ");

        if (message != null)
        {
            sb.Append($"Message: {message}");
        }

        output.WriteLine(sb.ToString());
        Debug.WriteLine(sb.ToString());

        return taskT;
    }

    public static Task Dump(this Task task, ITestOutputHelper output, string? message = null)
    {
        var sb = new StringBuilder();

        sb.Append($"{DateTime.UtcNow:dd MMM HH:mm:ss ffff} ");
        sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3}-{task.Id:D3} ");
        sb.Append($"Status: {task.Status} ");

        if (message != null)
        {
            sb.Append($"Message: {message}");
        }

        output.WriteLine(sb.ToString());
        Debug.WriteLine(sb.ToString());

        return task;
    }

    public static T Dump<T>(this T instance, ITestOutputHelper output, string? message = null)
    {
        var sb = new StringBuilder();

        sb.Append($"{DateTime.UtcNow:dd MMM HH:mm:ss ffff} ");
        sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3}-000 ");
        if (message != null)
        {
            sb.Append($"Message: {message}");
        }

        output.WriteLine(sb.ToString());
        Debug.WriteLine(sb.ToString());

        return instance;
    }

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}

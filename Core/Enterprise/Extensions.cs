using System.Threading.Tasks;
using Core.Enterprise.UserStory;
using Core.Enterprise.UserTasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Core.Enterprise;

public static class Extensions
{
    public static Task<T> Dump<T>(this Task<T> task, ITestOutputHelper output)
    {
        output.WriteLine(
            $"{DateTime.UtcNow:dd/MM/yyyy HH:mm:ss fff} " +
            $"ProcessId: {Environment.ProcessId:D5}; " +
            $"ThreadId: {Environment.CurrentManagedThreadId:D3}; " +
            $"TaskId: {task.Id:D3}; " +
            $"TaskStatus: {task.Status}; " +
            $"TaskResult: {task.Result}");

        return task;
    }

    public static Task Dump(this Task task, ITestOutputHelper output)
    {
        output.WriteLine(
            $"{DateTime.UtcNow:dd/MM/yyyy HH:mm:ss fff} " +
            $"ProcessId: {Environment.ProcessId:D5}; " +
            $"ThreadId: {Environment.CurrentManagedThreadId:D3}; " +
            $"TaskId: {task.Id:D3}; " +
            $"TaskStatus: {task.Status};");

        return task;
    }

    public static T Dump<T>(this T instance, ITestOutputHelper output, Func<T, string> toText = null)
    {
        toText ??= x => x.ToString();
        output.WriteLine(
            $"{DateTime.UtcNow:dd/MM/yyyy HH:mm:ss fff} " +
            $"ProcessId: {Environment.ProcessId:D5}; " +
            $"ThreadId: {Environment.CurrentManagedThreadId:D3}; " +
            $"Text: { toText(instance) }");

        return instance;
    }




    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}

using System.Text;
using Core.Enterprise.UserStory;
using Core.Enterprise.UserTasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Core.Enterprise;

public static class Extensions
{
    public static T Dump<T>(this T instance, ITestOutputHelper output, string? message = null)
    {
        var sb = new StringBuilder();

        sb.Append($"{DateTime.UtcNow:dd/MM/yyyy HH:mm:ss ffff} ");

        if (instance is Task task)
        {
            sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3}-{task.Id:D3} ");
            sb.Append($"Status: {task.Status} ");
        }
        else if (instance is Task<T> taskT)
        {
            sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3}-{taskT.Id:D3} ");
            sb.Append($"Status: {taskT.Status} ");
            sb.Append($"Result: {taskT.Result} ");
        }
        else
        {
            sb.Append($"Id: {Environment.ProcessId:D5}-{Environment.CurrentManagedThreadId:D3} ");
        }

        if (message != null)
        {
            sb.Append($"Message: {message}");
        }

        output.WriteLine(sb.ToString());

        return instance;
    }


    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddUserStory();
        services.AddFeatureTask();

        return services;
    }
}

using System.Diagnostics;
using System.Text;

namespace Design.Core;

public static class TestExtensions
{
    [DebuggerStepThrough]
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

    [DebuggerStepThrough]
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

    [DebuggerStepThrough]
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
}

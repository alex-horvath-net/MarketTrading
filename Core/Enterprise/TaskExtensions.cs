using System.Runtime.CompilerServices;
using Core.Enterprise;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Core.Enterprise;

public static class TaskExtensions
{
    public static Task<T> ToTask<T>(
        this T t) => Task.FromResult(t);

    public static CancellationToken NewToken(
        this CancellationToken _)
    {
        using var soure = new CancellationTokenSource();
        return soure.Token;
    }

    public static CancellationToken NewToken(
        this CancellationToken _,
        TimeSpan delay)
    {
        using var soure = new CancellationTokenSource(delay);
        return soure.Token;

    }

    public static CancellationToken NewToken(
        this CancellationToken _,
        TimeSpan delay,
        TimeProvider time)
    {
        using var soure = new CancellationTokenSource(delay, time);
        return soure.Token;
    }

    public async static void FireAndForget(
        this Task task,
        bool keepTheCallerThread = false,
        bool retrhrowException = true,
        Action? onCompleted = null,
        Action<Exception>? onException = null)
    {
        try
        {
            await task.ConfigureAwait(keepTheCallerThread);
            onCompleted?.Invoke();
        } catch (Exception ex)
        {
            onException?.Invoke(ex);

            if (retrhrowException)
                throw;
        }
    }

    public static async IAsyncEnumerable<R> Yield<T, R>(
        this IEnumerable<T> listT,
        Func<T, CancellationToken, Task<R>> t2TaskR,
        [EnumeratorCancellation] CancellationToken token)
    {
        var listTaskR = listT.Select(t => t2TaskR(t, token)).ToList();
        while (listTaskR.Count > 0)
        {
            var taskR = await Task.WhenAny(listTaskR).ConfigureAwait(false);
            listTaskR.Remove(taskR);
            var r = await taskR.ConfigureAwait(false);
            yield return r;
        }
    }


    public static async Task<T> Join<T>(
        this Task<Task<T>> nestedTaskT)
    {
        var taskT = await nestedTaskT;
        var t = await taskT;
        return t;
    }


    public async static Task<R> Select<T, R>(
        this Task<T> taskT,
        Func<T, R> mapT2R)
    {
        var t = await taskT;
        var r = mapT2R(t);
        return r;
    }


    public static async Task<R> SelectMany<T, R>(
        this Task<T> taskT,
        Func<T, Task<R>> mapT2TaskR)
    {
        var t = await taskT;
        var taskR = mapT2TaskR(t);
        var r = await taskR;
        return r;
        //taskT.Select(mapT2TaskR).Join<R>();
    }

    public async static Task<R> SelectMany<T, U, R>(
        this Task<T> taskT,
        Func<T, Task<U>> mapT2TaskU,
        Func<T, U, R> mapTU2R)
    {
        //Map(mapT2BoxU).Flatten<U>().Map(u => mapTU2R(Content, u));  //Bind(t => mapT2BoxU(t).Map(u => mapTU2R(t, u)));

        var t = await taskT;
        var u = await mapT2TaskU(t);
        var r = mapTU2R(t, u);
        return r;
    }
}

public class TaskDesign
{
    private readonly ITestOutputHelper output;
    private readonly CancellationToken token = CancellationToken.None;


    private string MapToItself(string itself) => itself;
    private int Parse(string text) => int.Parse(text);
    private DateTime ToDate(int year) => new DateTime(year, 1, 1);
    private async Task<string> GetYear()
    {
        this.Dump(output, "before delay");
        await Task.Delay(100).Dump(output, "during delay");
        this.Dump(output, "after delay");
        return "1984";
    }

    public TaskDesign(ITestOutputHelper output) => this.output = output;

    [Fact]
    public void Create_A_Task_From_A_Result()
    {
        var year = "1984";
        this.Dump(output);

        var task = year.ToTask().Dump(output);
        this.Dump(output);

        task.Should().NotBeNull();
        task.Should().BeOfType<Task<string>>();
        task.Result.Should().Be(year);

    }

    [Fact]
    public void Create_A_Task_From_A_Method()
    {
        this.Dump(output);

        var task = GetYear();
        task.Dump(output, "during task");

        this.Dump(output);

        task.Should().NotBeNull();
        task.IsCompleted.Should().BeFalse();

    }

    [Fact]
    public async Task Get_Result_Of_The_Task()
    {
        this.Dump(output, "before creating task");

        var task = GetYear().Dump(output, "creating task");

        this.Dump(output, "after creating task");

        this.Dump(output, "before waiting for task");

        var result = await task.ConfigureAwait(false).Dump(output, "during waiting for task");
        task.Dump(output, "after waiting for task");

        //task.Should().BeOfType<Task<string>>();
        result.Should().Be("1984");
    }

    [Fact]
    public async Task Change_Task_By_Lambda()
    {
        var oldTask = GetYear();

        var newTask = oldTask.Select(Parse);
        var result = await newTask;

        result.Should().Be(1984);
    }
    [Fact]
    public async Task Change_Task_By_Linq()
    {
        var task = "1984".ToTask();

        var result = await (
            from subResult in task
            select Parse(subResult));

        result.Should().Be(1984);
    }
    [Fact]
    public async void Map_Functor_1()
    {
        var task = "1984".ToTask();

        var result = await task.Select(MapToItself);

        result.Should().Be("1984");
    }
    [Fact]
    public async void Map_Functor_2()
    {
        var task = "1984".ToTask();

        var sequentialResult = await task.Select(Parse).Select(ToDate);
        var nestedResult = await task.Select(c => ToDate(Parse(c)));

        sequentialResult.Should().Be(nestedResult);
    }
    [Fact]
    public async void Join()
    {
        var task = "1984".ToTask();
        var nestedTask = task.ToTask();

        var result = await nestedTask.Join();

        result.Should().Be("1984");
    }
    [Fact]
    public async void SelectMany_Lambda()
    {
        var task = "1984".ToTask();

        var result = await task.SelectMany(text => Parse(text).ToTask());

        result.Should().Be(1984);
    }
    [Fact]
    public async void SelectMany_Linq()
    {
        var task = "1984".ToTask();

        var result = await (
            from result1 in task
            from result2 in Parse(result1).ToTask()
            select result2);

        result.Should().Be(1984);
    }

    [Fact]
    public async void FireAndForget_Ok()
    {
        this.Dump(output, "before");
        var task = Task.Delay(200, token).Dump(output, "during");
        var isCompleted = false;

        task.FireAndForget(onCompleted: () => { this.Dump(output, "completed"); isCompleted = true; });

        await Task.Delay(300, token);
        this.Dump(output, "after");
        task.IsCompleted.Should().BeTrue();
        isCompleted.Should().BeTrue();
    }

    [Fact]
    public async void FireAndForget_Fail()
    {
        this.Dump(output, "before");
        var task = Task.FromException(new Exception("TestException"));  // Task.Delay(200, token).Dump(output, "during");
        var isFailed = false;


        task.FireAndForget(retrhrowException: false, onException: ex => { this.Dump(output, ex.Message); isFailed = true; });

        this.Dump(output, "after");
        task.IsCompleted.Should().BeTrue();
        isFailed.Should().BeTrue();
    }


    [Fact]
    public async void Yield()
    {
        this.Dump(output, "before");

        Func<Task> slowTaskExecutions = async () =>
        {
            var token = CancellationToken.None;
            var nums = Enumerable.Range(0, 10).Select(_ => Random.Shared.Next(100, 500)).ToList();

            await foreach (var text in nums.Yield(NumToStringTask, token))
            {
                this.Dump(output, text);
            }
        };

        await slowTaskExecutions.Should().CompleteWithinAsync(2000.Milliseconds());

        this.Dump(output, "after");
    }



    private async Task<string> NumToStringTask(int num, CancellationToken token)
    {
        await Task.Delay(num, token);
        return $"{num:D3}";
    }

}

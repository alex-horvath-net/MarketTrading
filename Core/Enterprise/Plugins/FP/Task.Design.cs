using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace Core.Enterprise.Plugins.FP;

public class TaskDesign
{
    private readonly ITestOutputHelper output;

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

        var task = GetYear().Dump(output);

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
    public async void FireAndForget()
    {
        var random = new Random();
        var token = CancellationToken.None;
        this.Dump(output, "before");
        var task = Task.Delay(5000, token).Dump(output, "during");

        task.Start(onCompleted: () => this.Dump(output, "completed"));

        this.Dump(output, "after");
    }


    [Fact]
    public async void Yield()
    {
        var random = new Random();
        var token = CancellationToken.None;
        this.Dump(output, "before");

        List<int> nums2 = new();

        var nums = Enumerable.Range(0, 10).Select(_ => new Random().Next(200, 2000)).ToList();

        await foreach (var item in nums.Yield(async (n, t) => { await Task.Delay(n, t); return $"{n:D3}"; }, token))
        {
            this.Dump(output, item);
        }



        this.Dump(output, "after");
    }
}

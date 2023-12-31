using FluentAssertions;
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
        this.Dump(output, "GetYear before wait");
        await Task.Delay(100).Dump(output, "GetYear during wait");
        this.Dump(output, "GetYear after wait");
        return "1984";
    }

    public TaskDesign(ITestOutputHelper output) => this.output = output;

    [Fact]
    public void Create_A_Task_From_A_Result()
    {
        var year = "1984";
        this.Dump(output, "before task");
        var task = year.ToTask().Dump(output, "during task");
        this.Dump(output, "after task");
        task.Should().NotBeNull();
        task.Should().BeOfType<Task<string>>();
        task.Result.Should().Be(year);

    }

    [Fact]
    public void Create_A_Task_From_A_Method()
    {
        var task = GetYear().Dump(output);

        task.Should().NotBeNull();
        task.IsCompleted.Should().BeFalse();

    }

    [Fact]
    public async void Get_Result_Of_The_Task()
    {
        var task = GetYear();

        var result = await task.Dump(output);
        task.Dump(output);

        task.Should().BeOfType<Task<string>>();
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
    public async Task Select_LinqAsync()
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
}

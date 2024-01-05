using FluentAssertions.Extensions;

namespace Design.Core.Enterprise;

public class Task_Design : Design<Task_Design>
{
    public Task_Design(ITestOutputHelper output) : base(output) { }

    private string MapToItself(string itself) => itself;
    private int Parse(string text) => int.Parse(text);
    private DateTime ToDate(int year) => new DateTime(year, 1, 1);
    private async Task<string> GetYear()
    {
        this.Dump(Output, "before delay");
        await Task.Delay(100).Dump(Output, "during delay");
        this.Dump(Output, "after delay");
        return "1984";
    }


    [Fact]
    public void Create_A_Task_From_A_Result()
    {
        var year = "1984";
        this.Dump(Output);

        var task = year.ToTask().Dump(Output);
        this.Dump(Output);

        task.Should().NotBeNull();
        task.Should().BeOfType<Task<string>>();
        task.Result.Should().Be(year);

    }

    [Fact]
    public void Create_A_Task_From_A_Method()
    {
        this.Dump(Output);

        var task = GetYear();
        task.Dump(Output, "during task");

        this.Dump(Output);

        task.Should().NotBeNull();
        task.IsCompleted.Should().BeFalse();

    }

    [Fact]
    public async Task Get_Result_Of_The_Task()
    {
        this.Dump(Output, "before creating task");

        var task = GetYear().Dump(Output, "creating task");

        this.Dump(Output, "after creating task");

        this.Dump(Output, "before waiting for task");

        var result = await task.ConfigureAwait(false).Dump(Output, "during waiting for task");
        task.Dump(Output, "after waiting for task");

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
        this.Dump(Output, "before");
        var task = Task.Delay(200, Token).Dump(Output, "during");
        var isCompleted = false;

        task.FireAndForget(onCompleted: () => { this.Dump(Output, "completed"); isCompleted = true; });

        await Task.Delay(300, Token);
        this.Dump(Output, "after");
        task.IsCompleted.Should().BeTrue();
        isCompleted.Should().BeTrue();
    }

    [Fact]
    public void FireAndForget_Fail()
    {
        this.Dump(Output, "before");
        var task = Task.FromException(new Exception("TestException"));  // Task.Delay(200, token).Dump(Output, "during");
        var isFailed = false;


        task.FireAndForget(retrhrowException: false, onException: ex => { this.Dump(Output, ex.Message); isFailed = true; });

        this.Dump(Output, "after");
        task.IsCompleted.Should().BeTrue();
        isFailed.Should().BeTrue();
    }


    [Fact]
    public async void Yield()
    {
        this.Dump(Output, "before");

        Func<Task> slowTaskExecutions = async () =>
        {
            var token = CancellationToken.None;
            var nums = Enumerable.Range(0, 10).Select(_ => Random.Shared.Next(100, 500)).ToList();

            await foreach (var text in nums.Yield(NumToStringTask, token))
            {
                this.Dump(Output, text);
            }
        };

        await slowTaskExecutions.Should().CompleteWithinAsync(2000.Milliseconds());

        this.Dump(Output, "after");
    }

    private async Task<string> NumToStringTask(int num, CancellationToken token)
    {
        await Task.Delay(num, token);
        return $"{num:D3}";
    }
}

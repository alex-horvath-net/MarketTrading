using System.Diagnostics;
using System.Runtime.CompilerServices;
using Core;
using Microsoft.Extensions.Logging;

namespace Common.Plugins.TaskTry;

public class Game
{
    private readonly ILogger<Game> logger;
    private readonly Random spining = new();
    private readonly Random dice = new();
    public Game(ILogger<Game> logger)
    {
        this.logger = logger;
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Game is created.");
    }
    public async Task Play(CancellationToken token)
    {
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Start play.");

        await foreach(var num in DropTimes(10, token).ConfigureAwait(false))
        {
            logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Num is {num}");
        }

        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Finish play.");


    }

    private async IAsyncEnumerable<int> DropTimes(int times, [EnumeratorCancellation] CancellationToken token)
    {
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} start Drop.");

        var intTasks = Enumerable.Range(0, times).Select(i => DropDice(i, token)).ToList();

        while (intTasks.Any())
        {
            var completedTask = await Task.WhenAny(intTasks).ConfigureAwait(false);
            intTasks.Remove(completedTask);
            yield return await completedTask.ConfigureAwait(false);
        }

        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} finish drop.");
    }

    

    private async Task<int> DropDice(int dropAccasion, CancellationToken token)
    {
        await Delay(token).ConfigureAwait(false); 
        int nextNum = GetNum();
        return nextNum;

    }

    private int GetNum()
    {
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} start GetNum.");
        var nextNum = dice.Next(1, 6 + 1);
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} end GetNum. Num is {nextNum}");
        return nextNum;
    }

    private async Task Delay(CancellationToken token)
    {
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} start delay.");
        var delayInMs = spining.Next(200, 1000);
        await Task.Delay(delayInMs, token).ConfigureAwait(false);
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} stop delay. Duration {delayInMs} ms.");
    }
}

using System.Runtime.CompilerServices;
using Core.Enterprise;
using Microsoft.Extensions.Logging;

namespace Core.Application.Plugins.TaskTry;

public class Game
{
    private readonly ILogger<Game> logger;
    private readonly Random random = new();
    public Game(ILogger<Game> logger)
    {
        this.logger = logger;
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Game is created.");
    }
    public async Task Play(int count, [EnumeratorCancellation] CancellationToken token)
    {
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Start play.");

        //await foreach (var num in GetNums(count, token).ConfigureAwait(false))
        await foreach (var num in Enumerable.Range(0, count).Yield(GetNum, token).ConfigureAwait(false))
        {
            logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Num is {num}");
        }

        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} Finish play.");


    }

    private async Task<int> GetNum(int position, CancellationToken token)
    {
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} start GetNum, position: {position}.");
        var num = random.Next(200, 2000);
        await Task.Delay(num, token).ConfigureAwait(false);
        logger.LogInformation($"{Thread.CurrentThread.ManagedThreadId} end GetNum., position: {position}.");
        return num;
    }

}

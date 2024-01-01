using System.Runtime.CompilerServices;

namespace Core.Enterprise.Plugins.FP;

public static class TaskExtensions
{
    public static Task<T> ToTask<T>(this T t) => Task.FromResult(t);

    public static async Task<T> Join<T>(this Task<Task<T>> nestedTaskT)
    {
        var taskT = await nestedTaskT;
        var t = await taskT;
        return t;
    }

    public async static Task<R> Select<T, R>(this Task<T> taskT, Func<T, R> mapT2R)
    {
        var t = await taskT;
        var r = mapT2R(t);
        return r;
    }


    public static async Task<R> SelectMany<T, R>(this Task<T> taskT, Func<T, Task<R>> mapT2TaskR)
    {
        var t = await taskT;
        var taskR = mapT2TaskR(t);
        var r = await taskR;
        return r;
        //taskT.Select(mapT2TaskR).Join<R>();
    }
    public async static Task<R> SelectMany<T, U, R>(this Task<T> taskT, Func<T, Task<U>> mapT2TaskU, Func<T, U, R> mapTU2R)
    {
        //Map(mapT2BoxU).Flatten<U>().Map(u => mapTU2R(Content, u));  //Bind(t => mapT2BoxU(t).Map(u => mapTU2R(t, u)));

        var t = await taskT;
        var u = await mapT2TaskU(t);
        var r = mapTU2R(t, u);
        return r;
    }

    public async static void Start(this Task task,
        bool returnToCallerTread = false,
        bool retrhrowException = true,
        Action? onCompleted = null,
        Action<Exception>? onException = null)
    {
        try
        {
            await task.ConfigureAwait(returnToCallerTread);
            onCompleted?.Invoke();
        } catch (Exception ex)
        {
            onException?.Invoke(ex);

            if (retrhrowException)
                throw;
        }
    }

    public static async IAsyncEnumerable<R> Yield<T, R>(this IEnumerable<T> listT, 
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
}
namespace Core.Sys;

public class CancellationTokenBuilder : IDisposable
{
    public CancellationTokenBuilder CancelAfter(TimeSpan delay) => CancelAfter(delay, TimeProvider.System);

    public CancellationTokenBuilder CancelAfter(TimeSpan delay, TimeProvider time)
    {
        source = new(delay, time);
        return this;
    }

    public void Dispose() => source.Dispose();

    public CancellationToken Token => source.Token;
    public CancellationToken None => CancellationToken.None;

    private CancellationTokenSource source = new();
}

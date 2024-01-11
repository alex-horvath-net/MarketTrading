namespace AppPolicy;

public class CancellationTokenBuilder() : IDisposable {
    public CancellationTokenBuilder Schedule(TimeSpan delay, TimeProvider? time = null) {
        this.delay = delay;
        this.time = time ?? TimeProvider.System;
        return this;
    }

    public CancellationTokenBuilder LinkTo(CancellationToken token) {
        source = CancellationTokenSource.CreateLinkedTokenSource(token);
        return this;
    }

    public CancellationToken Build() {
        source ??=
            delay == TimeSpan.Zero ?
            new CancellationTokenSource() :
            new CancellationTokenSource(delay, time);

        return source.Token;
    }

    public void Dispose() => source?.Dispose();

    public static implicit operator CancellationToken(CancellationTokenBuilder builder) => builder.Build();

    private CancellationTokenSource? source;
    private TimeSpan delay = TimeSpan.Zero;
    private TimeProvider time = TimeProvider.System;
}

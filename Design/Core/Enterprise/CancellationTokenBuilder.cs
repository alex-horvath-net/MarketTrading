using Core.Enterprise;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Time.Testing;

namespace Design.Core.Enterprise;

public class CancellationTokenBuilder_Design
{
    private void Create() => Unit = new();
    private void CancelAfterDelay() => Unit.CancelAfter(delay);
    private void CancelAfterDelayWithTime() => Unit.CancelAfter(delay, Time);

    public CancellationTokenBuilder_Design(ITestOutputHelper output) => Output = output;

    [Fact]
    public void ItRequires_NoDependecies()
    {
        Create();

        Unit.Should().NotBeNull();
    }

    [Fact]
    public void ItCan_ProvideNoneCancelableToken()
    {
        Create();

        var token = Unit.None;

        token.CanBeCanceled.Should().BeFalse();
        token.IsCancellationRequested.Should().BeFalse();
    }

    [Fact]
    public void ItCan_ProvideNewToken()
    {
        Create();

        var newToken = Unit.Token;

        newToken.Should().NotBeNull();
        newToken.CanBeCanceled.Should().BeTrue();
        newToken.IsCancellationRequested.Should().BeFalse();
    }

    [Fact]
    public async Task ItCan_ProvideNewTokenWithScheduledCancelation()
    {
        Create();

        CancelAfterDelay();

        var newToken = Unit.Token;
        await Task.Delay(300.Milliseconds());
        newToken.IsCancellationRequested.Should().BeTrue();
    }

    [Fact]
    public void ItCan_ProvideNewTokenWithScheduledCancelationAndTime()
    {
        Create();

        CancelAfterDelayWithTime();

        var newToken = Unit.Token;
        Time.Advance(300.Milliseconds());

        newToken.IsCancellationRequested.Should().BeTrue();
    }

    public void Dispose() => Unit?.Dispose();

    private readonly ITestOutputHelper Output;
    private readonly FakeTimeProvider Time = new();
    private CancellationTokenBuilder Unit { get; set; }
    private TimeSpan delay = 200.Milliseconds();
}

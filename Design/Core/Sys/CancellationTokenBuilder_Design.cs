using AppPolicy;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Time.Testing;

namespace Design.AppPolicy;

public class CancellationTokenBuilder_Design :Design<CancellationTokenBuilder>
{
    private void Create() => Unit = new();
    private void Schedule() => Unit.Schedule(delay);
    private void ScheduleWithTime() => Unit.Schedule(delay, Time);
     
    public CancellationTokenBuilder_Design(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void ItRequires_NoDependecies()
    {
        Create();

        Unit.Should().NotBeNull();
    }

    [Fact]
    public void ItCan_ProvideNewToken()
    {
        Create();

        var newToken = Unit.Build();

        newToken.Should().NotBeNull();
        newToken.CanBeCanceled.Should().BeTrue();
        newToken.IsCancellationRequested.Should().BeFalse();
    }

    [Fact]
    public async Task ItCan_ProvideNewTokenWithScheduledCancelation()
    {
        Create();

        Schedule();

        var newToken = Unit.Build();
        await Task.Delay(300.Milliseconds());
        newToken.IsCancellationRequested.Should().BeTrue();
    }

    [Fact]
    public void ItCan_ProvideNewTokenWithScheduledCancelationAndTime()
    {
        Create();

        ScheduleWithTime();

        var newToken = Unit.Build();
        Time.Advance(300.Milliseconds());

        newToken.IsCancellationRequested.Should().BeTrue();
    }

    public void Dispose() => Unit?.Dispose();

    private readonly ITestOutputHelper Output;
    private readonly FakeTimeProvider Time = new();
    
    private TimeSpan delay = 200.Milliseconds();
}

using Xunit.Abstractions;
using Microsoft.Extensions.Time.Testing;

namespace Core.Enterprise;

public class Design<T>
{
    protected T Unit { get; set; }
    protected readonly ITestOutputHelper Output;
    protected readonly FakeTimeProvider Time = new();
    protected CancellationTokenBuilder TokenBuilder = new();
    protected CancellationToken Token => TokenBuilder.Token;

    protected Design(ITestOutputHelper output) => this.Output = output;
}

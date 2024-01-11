using Core;
using Microsoft.Extensions.Time.Testing;

namespace Design.Core;

public class Design<T>
{
    protected T Unit { get; set; }
    protected readonly ITestOutputHelper Output;
    protected readonly FakeTimeProvider Time = new();
    protected CancellationTokenBuilder Token = new();

    protected Design(ITestOutputHelper output) => Output = output;
}

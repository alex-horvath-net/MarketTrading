using Xunit.Abstractions;
using Microsoft.Extensions.Time.Testing;

namespace Core.Enterprise;

public class Design<T>
{
    protected Design(ITestOutputHelper output) => this.Output = output;
    protected readonly ITestOutputHelper Output;
    protected readonly FakeTimeProvider Time = new();
    protected T Unit { get; set; }
    protected CancellationTokenBuilder Token = new(); 
}

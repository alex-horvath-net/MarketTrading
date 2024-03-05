using Microsoft.Extensions.Time.Testing;

namespace Core;

public class UnitDesign<TUnit> {
    protected TUnit? Unit { get; set; }
    protected readonly ITestOutputHelper Output;
    protected readonly FakeTimeProvider Time = new();
    protected CancellationTokenBuilder token = new();

    protected UnitDesign(ITestOutputHelper output) => Output = output;
}

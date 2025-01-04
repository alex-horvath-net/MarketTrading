
namespace BusinesActors.Trader; 
public class TraderScope {
    [Fact]
    public void I_Can_Find_All_Trades() {
        var trader = new Trader();

        var trades = trader.FindAllTransations();

        trades.Should().NotBeEmpty();
    }
}

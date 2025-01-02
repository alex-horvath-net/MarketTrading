using Experts.Trader;

namespace Tests {
    public class Trader_Says {
        [Fact]
        public void I_Can_Find_All_Trades() {
            var trader = new Trader();
            
            var trades = trader.FindAllTrade();
            
            trades.Should().NotBeEmpty();
        }
    }
}

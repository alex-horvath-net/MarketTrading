namespace Tests {
    public class Tradaer_Test {
        [Fact] void Test() {
            var trader = new Experts.Trader.Trader();
            var transaction = trader.FindAllTransation();
            transaction.Should().NotBeEmpty();
        }
    }
}

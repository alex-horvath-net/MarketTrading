using Infrastructure.Business.Model;

namespace Experts.Trader {
    public class Trader {
        public IEnumerable<Trade> FindAllTrade() {
            return [new() { Name = "Name1" }];
        }
    }
}
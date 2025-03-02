using YourBank.Business.Domain;

namespace YourBank.Business.Experts.Trader {
    public class Trader {
        public IEnumerable<Trade> FindAllTransations() {
            return [new() { Name = "Name1" }];
        }
    }
}
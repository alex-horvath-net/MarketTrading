using Business.Domain;

namespace Business.Experts.Trader {
    public class Trader {
        public IEnumerable<Trade> FindAllTransations() {
            return [new() { Name = "Name1" }];
        }
    }
}
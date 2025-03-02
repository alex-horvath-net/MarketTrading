using BusinessDomain;

namespace BusinesActors.Trader {
    public class Trader {
        public IEnumerable<Trade> FindAllTransations() {
            return [new() { Name = "Name1" }];
        }
    }
}
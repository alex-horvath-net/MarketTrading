using Business.Domain;

namespace Business.Experts.Trader {
    public class Expert {
        public IEnumerable<Trade> FindAllTransations() {
            return [new() { Name = "Name1" }];
        }
    }
}
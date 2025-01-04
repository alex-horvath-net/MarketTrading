using System.ComponentModel;
using Infrastructure.Adapters.Blazor;

namespace BusinesActors.Trader.FindTransactions.Triggers.Blazor.InputPort;

public record ViewModel {
    public MetaVM Meta { get; set; }
    public List<ErrorVM> Errors { get; set; } = [];
    public DataListModel<TransactionVM> Transactions { get; set; }
    public class MetaVM {
        public Guid Id { get; internal set; }
    }

    public class ErrorVM {
        public string Name { get; internal set; }
        public string Message { get; internal set; }
    }

    public record TransactionVM {
        [DisplayName("ID")]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}






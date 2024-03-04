namespace Core.Business.Model;

public record ResponseCore<TRequest> where TRequest : RequestCore {
    public MetaDataCore<TRequest> MetaData { get; } = new();

}


public record MetaDataCore<TRequest>()
    where TRequest : RequestCore {
    public DateTime StartedAt { get; set; }
    public DateTime? Stoped { get; set; }
    public bool Enabled { get; set; }
    public TRequest Request { get; set; }
    public IEnumerable<Result> RequestIssues { get; set; } = [];
}
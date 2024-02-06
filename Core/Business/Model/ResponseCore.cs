namespace Core.Business.Model;

public record ResponseCore<TRequest>()
    where TRequest : RequestCore {
    public MetaDataCore<TRequest> MetaData { get; set; } = new();

}


public record MetaDataCore<TRequest>()
    where TRequest : RequestCore {
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool Enabled { get; set; }
    public TRequest Request { get; set; }
    public IEnumerable<Result> Results { get; set; } = [];
}
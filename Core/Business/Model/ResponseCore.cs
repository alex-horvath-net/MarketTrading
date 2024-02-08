namespace Core.Business.Model;

public record ResponseCore<TRequest>()
    where TRequest : RequestCore {
    public MetaDataCore<TRequest> MetaData { get; set; } = new();

}


public record MetaDataCore<TRequest>()
    where TRequest : RequestCore {
    public DateTime Start { get; set; }
    public DateTime? Stop { get; set; }
    public bool Enabled { get; set; }
    public TRequest Request { get; set; }
    public IEnumerable<Result> RequestIssues { get; set; } = [];
}
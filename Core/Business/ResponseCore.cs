namespace Core.Business;

public record ResponseCore<TRequest>()
    where TRequest : RequestCore {
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool FeatureEnabled { get; set; }
    public TRequest Request { get; set; }
    public IEnumerable<ValidationResult> ValidationResults { get; set; }
}
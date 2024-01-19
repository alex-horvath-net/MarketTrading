namespace Core.Story.StoryModel;

public record Response<TRequest>()
    where TRequest : Request {
    public DateTime StartedAt { get; set; }
    public DateTime EndAt { get; set; }
    public bool FeatureEnabled { get; set; }
    public TRequest Request { get; set; }
    public bool Terminated { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
}
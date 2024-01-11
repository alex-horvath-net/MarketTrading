namespace Core.UserStory.DomainModel;

public record Response<TRequest>()
    where TRequest : Request {
    public DateTime StartedAt { get; set; }
    public DateTime EndAt { get; set; }
    public bool FeatureEnabled { get; set; }
    public TRequest Request { get; set; }
    public bool Terminated { get; set; }
    public IEnumerable<ValidationDomainModel> Validations { get; set; }
}
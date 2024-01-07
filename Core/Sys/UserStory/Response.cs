namespace Core.Sys.UserStory;

public record Response<TRequest>()
    where TRequest : Request
{
    public bool FeatureEnabled { get; set; }
    public TRequest Request { get; set; }
    public bool Terminated { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
}

namespace Core.Enterprise.UserStory;

public record ResponseCore<TRequest>()
    where TRequest : RequestCore
{
    public bool FeatureEnabled { get; set; }
    public TRequest? Request { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
}

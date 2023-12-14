namespace Core.UserStory;

public record ResponseCore<TRequest>()
    where TRequest : RequestCore
{
    public bool FeatureFlag { get; set; }

    public TRequest Request { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
    public bool CanRun { get; set; }
}

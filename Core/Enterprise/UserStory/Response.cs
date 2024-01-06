namespace Core.Enterprise.UserStory;

public record Response<TRequest>()
    where TRequest : RequestCore
{
    public bool FeatureEnabled { get; set; }
    public TRequest Request { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
}

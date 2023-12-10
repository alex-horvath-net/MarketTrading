namespace Core.UserStory.UserStoryUnit;

public record ResponseCore<TRequest>()
    where TRequest : RequestCore
{
    public TRequest Request { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
    public bool Stopped { get; set; }
}

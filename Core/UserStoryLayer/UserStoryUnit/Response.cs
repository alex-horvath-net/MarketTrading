namespace Principals.UserStoryLayer.UserStoryUnit;

public record Response<TRequest>() where TRequest : Request
{
    public TRequest Request { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
    public bool Stopped { get; set; }
}

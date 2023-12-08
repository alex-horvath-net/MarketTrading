namespace Sys.UserStory;

public abstract class UserStoryCore<TRequest, TResponse>(IEnumerable<ITask<TResponse>> workSteps)
         : IUserStory<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()

{
    public async Task<TResponse> Run(TRequest request, CancellationToken cancellation)
    {
        var response = new TResponse() with { Request = request };
        foreach (var workStep in workSteps)
        {
            await workStep.Run(response, cancellation);
            cancellation.ThrowIfCancellationRequested();
            if (response.Stopped) return response;
        }
        return response;
    }
}

public record RequestCore();

public record ResponseCore<TRequest>() where TRequest : RequestCore
{
    public TRequest Request { get; set; }
    public IEnumerable<ValidationResult> Validations { get; set; }
    public bool Stopped { get; set; }
}
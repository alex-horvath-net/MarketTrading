using System.ComponentModel.DataAnnotations;
using Azure.Core;

namespace Core.Business;

public interface IStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}

public class Story<TRequest, TResponse>(IValidation<TRequest> validator) : IStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public async Task<TResponse> Run(TRequest request, CancellationToken token) {
        var response = new TResponse();

        response.Request = request;
        response.StartedAt = DateTime.UtcNow;
        response.FeatureEnabled = false;
        response.Terminated = !response.FeatureEnabled;

        response.ValidationResults = await validator.Validate(response.Request, token);
        response.Terminated = response.ValidationResults.Any(x => !x.IsSuccess);
        if (!response.Terminated)
            await RunCore(response, token);

        response.EndAt = DateTime.UtcNow;

        return response;
    }

    public async Task BusinessTask(string name, TResponse response, Func<TResponse, Task> task) {

        await task(response);
    }

    public virtual Task RunCore(TResponse response, CancellationToken token) => Task.CompletedTask;
}


public interface IValidation<TRequest> where TRequest : Request {
    Task<IEnumerable<ValidationResult>> Validate(TRequest request, CancellationToken token);
}


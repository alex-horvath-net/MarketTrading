using Core.ExpertStory.DomainModel;

namespace Core.ExpertStory;

public class ExpertStory<TRequest, TResponse>(IEnumerable<IScope<TRequest, TResponse>> expertTasks) : IExpertStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public async Task<TResponse> Run(TRequest request, CancellationToken token) {
        var response = new TResponse() { Request = request };
        foreach (var expertTask in expertTasks) {
            await expertTask.Run(response, token);
            if (response.Terminated)
                break;
        }
        return response;
    }
}

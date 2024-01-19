using Core.Problems;
using Core.Story.Model;

namespace Core.Story;

public class Story<TRequest, TResponse>(IEnumerable<IProblem<TRequest, TResponse>> problems) : IStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    public async Task<TResponse> Run(TRequest request, CancellationToken token) {
        var response = new TResponse() { Request = request };

        var started = new Started<TRequest, TResponse>();
        await started.Run(response, token);

        var feature = new FeatureEnabled<TRequest, TResponse>();
        await feature.Run(response, token);

        foreach (var problem in problems.OrderBy(x => x.Order)) {
            if (response.Terminated)
                break;

            await problem.Run(response, token);
        }

        var completed = new Completed<TRequest, TResponse>();
        await completed.Run(response, token);

        return response;
    }
}

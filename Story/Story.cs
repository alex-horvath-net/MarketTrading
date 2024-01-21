using Azure;
using Story.Model;
using Story.Problems;

namespace Common;

//public class Story<TRequest, TResponse>(IEnumerable<IProblem<TRequest, TResponse>> problems) : IStory<TRequest, TResponse>
public class Story<TRequest, TResponse>() : IStory<TRequest, TResponse>
    where TRequest : Model.Request
    where TResponse : Model.Response<TRequest>, new() {
    public async Task<TResponse> Run(TRequest request, CancellationToken token) {
        var response = new TResponse() { Request = request };

        var started = new Started<TRequest, TResponse>();
        await started.Run(response, token);

        var feature = new FeatureEnabled<TRequest, TResponse>();
        await feature.Run(response, token);

        await RunCore(response, token);
        //await RunCore(problems, response, token);

        var completed = new Completed<TRequest, TResponse>();
        await completed.Run(response, token);

        return response;
    }

    public async Task ExpertTask(string name, TResponse response, Func<TResponse, Task> task) {

        await task(response);
    }

    public virtual Task RunCore(TResponse response, CancellationToken token) => Task.CompletedTask;


    //public virtual async Task RunCore(IEnumerable<IProblem<TRequest, TResponse>> problems, TResponse response, CancellationToken token) {
    //    foreach (var problem in problems.OrderBy(x => x.Order)) {
    //        if (response.Terminated)
    //            break;

    //        await problem.Run(response, token);
    //    }
    //}
}

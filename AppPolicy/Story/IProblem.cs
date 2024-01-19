using Core.Story.StoryModel;

namespace Core.Story;

public interface IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    Task Run(TResponse response, CancellationToken token);
}

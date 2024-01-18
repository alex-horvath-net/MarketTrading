using Core.ExpertStory.StoryModel;

namespace Core.ExpertStory;

public interface IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    Task Run(TResponse response, CancellationToken token);
}

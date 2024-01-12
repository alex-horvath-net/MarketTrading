using Core.ExpertStory.DomainModel;

namespace Core.ExpertStory;

public interface IScope<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    Task Run(TResponse response, CancellationToken token);
}

using Core.ExpertStory.DomainModel;

namespace Core.ExpertStory;

public interface IExpertStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}
 
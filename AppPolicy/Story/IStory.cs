using Core.Story.Model;

namespace Core.Story;

public interface IStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}

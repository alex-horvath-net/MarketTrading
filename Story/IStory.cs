using Story.Model;

namespace Story;

public interface IStory<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    Task<TResponse> Run(TRequest request, CancellationToken token);
}

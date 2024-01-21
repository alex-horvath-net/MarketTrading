using Story.Model;

namespace Story;

public interface IProblem<TRequest, TResponse>
    where TRequest : Request
    where TResponse : Response<TRequest>, new() {
    int Order => 0;
    int Group => 0;
    Task Run(TResponse response, CancellationToken token);
}

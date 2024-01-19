using Experts.Blogger.ReadPosts.Model;
using Story;

namespace Experts.Blogger.ReadPosts.Presentation;

public class Problem(ISolution solution) : IProblem<Request, Response> {
    public Task Run(Response response, CancellationToken token) => solution.Present(response, token);
}

public interface ISolution {
    Task Present(Response response, CancellationToken token);
}
using Experts.Blogger.ReadPosts.Model;
using Story;

namespace Experts.Blogger.ReadPosts.Read;

public class Problem(ISolution solution) : IProblem< Request, Response> {
    public async Task Run(Response response, CancellationToken token) {
        response.Posts = await solution.Read(response.Request, token);
    }
}

public interface ISolution {
    Task<IEnumerable<Story.Model.Post>> Read(Request Request, CancellationToken token);
}
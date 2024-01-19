using Experts.Blogger.ReadPosts.Model;

namespace Experts.Blogger.ReadPosts.Presentation;

public class Solution() : ISolution {
    public Task Present(Response response, CancellationToken token) {

        return Task.CompletedTask;
    }
}
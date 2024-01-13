using Core.ExpertStory;

namespace Experts.Blogger.ReadPosts.Read;

public class ExpertTask(ISolution solution) : IExpertTask<Request, Response> {
    public async Task Run(Response response, CancellationToken token) => 
        response.Posts = await solution.Read(response.Request, token);
}

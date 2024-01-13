using Common.Scope.ScopeModel;
using Core.ExpertStory;
//                Expert  Story     Task  
namespace Experts.Blogger.ReadPosts.Read;

public class ExpertTask(ISolution solution) : IExpertTask<Request, Response> {
    public async Task Run(Response response, CancellationToken token) => 
        response.Posts = await solution.Read(response.Request, token);
}

 
public interface ISolution {
    Task<IEnumerable<Post>> Read(Request Request, CancellationToken token);
}

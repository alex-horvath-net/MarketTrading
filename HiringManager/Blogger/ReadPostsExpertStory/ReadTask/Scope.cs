using Common.UserStory.DomainModel;
using Core.UserStory;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class Scope(ISolutionExpert expert) : IScope<Request, Response>
{
    public async Task Run(Response response, CancellationToken token) =>
        response.Posts = await expert.Read(response.Request, token);
}


public interface ISolutionExpert {
    Task<List<Post>> Read(Request request, CancellationToken token);
}
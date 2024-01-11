using Common.ExpertStories.DomainModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class SolutionExpert(ISolution solution) : ISolutionExpert
{
    public async Task<List<Post>> Read(Request request, CancellationToken token)
    {
        var expertModel = await solution.Read(request.Title, request.Content, token);
        var storyModel = expertModel.Select(model => new Post()
        {
            Title = model.Title,
            Content = model.Content
        }).ToList(); 
        return storyModel;
    }

    public async Task<IEnumerable<Post>> Read2(Request request, CancellationToken token) => 
            from expertModel in await solution.Read(request.Title, request.Content, token)
            select new Post() {
                Title = expertModel.Title,
                Content = expertModel.Content
            };
}


public interface ISolution {
    Task<List<Common.SolutionExperts.DataModel.Post>> Read(string title, string content, CancellationToken token);
}




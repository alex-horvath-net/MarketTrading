using Common.Plugins.DataAccess;
using Microsoft.EntityFrameworkCore;
using Common.Sockets.DataModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class Solution(DB entityFramework) : ISolution {
    public async Task<List<Post>> Read(string title, string content, CancellationToken token) {
        var solutionModel = await entityFramework
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(token);

        var expertModel = solutionModel.Select(model => model).ToList();
        return expertModel;
    }
}

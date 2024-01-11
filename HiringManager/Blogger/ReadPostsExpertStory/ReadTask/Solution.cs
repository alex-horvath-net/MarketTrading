using Microsoft.EntityFrameworkCore;
using Common.Solutions.DataAccess;
using Common.Models.DataModel;

namespace BusinessExperts.Blogger.ReadPostsExpertStory.ReadTask;

public class Solution(DB db) : ISolution {
    public async Task<IEnumerable<Post>> Read(string title, string content, CancellationToken token) {
        var technologyModel = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(title) || post.Content.Contains(content))
            .ToListAsync(token);

        var solutionModel = technologyModel.Select(model => model);
        return solutionModel;
    }
}

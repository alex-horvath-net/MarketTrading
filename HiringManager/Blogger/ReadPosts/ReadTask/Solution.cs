using Common.Solutions.Data.MainDB;
using Common.Solutions.Data.MainDB.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Experts.Blogger.ReadPosts.ReadTask;

public class Solution(MainDB db) : ISolution {
    public async Task<IEnumerable<Post>> Read(Request request, CancellationToken token) {
        var technologyModel = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
            .ToListAsync(token);

        var solutionModel = technologyModel.Select(model => model);
        return solutionModel;
    }
}

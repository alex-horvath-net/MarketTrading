using Common.ExpertStrory.StoryModel;
using Common.Solutions.Data.MainDB;
using Core;
using Microsoft.EntityFrameworkCore;

namespace Experts.Blogger.ReadPosts.Read;

public class Solution(MainDB db) : ISolution {
    public async Task<IEnumerable<Post>> Read(Request request, CancellationToken token) {
        var technology = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
            .ToListAsync(token);

        var model = technology.Select(tech => tech);

        var problem = model.Select(ToScopeModel);

        return problem;
    }

    private Post ToScopeModel(Common.Solutions.Data.MainDB.DataModel.Post solutionModel) => new() {
        Title = solutionModel.Title,
        Content = solutionModel.Content
    };
}
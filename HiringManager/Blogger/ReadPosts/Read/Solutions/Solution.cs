using Common.Solutions.Data.MainDB;
using Microsoft.EntityFrameworkCore;

namespace Experts.Blogger.ReadPosts.Read.Solutions;

public class Solution(MainDB db) : ISolution
{
    public async Task<IEnumerable<Common.Scope.ScopeModel.Post>> Read(Request request, CancellationToken token)
    {
        var technologyModel = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
            .ToListAsync(token);

        var solutionModel = technologyModel.Select(model => model);
        
        var scopeModel = solutionModel.Select(ToScopeModel);
        
        return scopeModel;
    }

    private Common.Scope.ScopeModel.Post ToScopeModel(Common.Solutions.Data.MainDB.DataModel.Post solutionModel) => new() {
        Title = solutionModel.Title,
        Content = solutionModel.Content
    };
}

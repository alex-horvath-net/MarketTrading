using Core;
using Experts.Blogger.ReadPosts.Model;
using Microsoft.EntityFrameworkCore;
using Story.Problem.Model;
using Story.Solutions.Data.MainDB;

namespace Experts.Blogger.ReadPosts.Read;

public class Solution(MainDB db) : ISolution {
    public async Task<IEnumerable<Post>> Read(Request request, CancellationToken token) {
        var solutionModel = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
            .ToListAsync(token);

        var problemModel = solutionModel
            .Select(model => new Post() {
                Title = model.Title,
                Content = model.Content
            });

        return problemModel;
    }
}
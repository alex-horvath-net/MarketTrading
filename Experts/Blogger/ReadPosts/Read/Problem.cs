using Experts.Blogger.ReadPosts.Model;
using Microsoft.EntityFrameworkCore;
using Story;
using Story.Solutions.Data.MainDB;

namespace Experts.Blogger.ReadPosts.Read;

public class Problem(ISolution solution) : IProblem< Request, Response> {
    public async Task Run(Response response, CancellationToken token) {
        response.Posts = await solution.Read(response.Request, token);
    }
}


public interface ISolution {
    Task<IEnumerable<Story.Model.Post>> Read(Request Request, CancellationToken token);
}


public class Solution(MainDB db) : ISolution {
    public async Task<IEnumerable<Story.Model.Post>> Read(Request request, CancellationToken token) {
        var solutionModel = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
            .ToListAsync(token);

        var problemModel = solutionModel
            .Select(model => new Story.Model.Post() {
                Title = model.Title,
                Content = model.Content
            });

        return problemModel;
    }
}
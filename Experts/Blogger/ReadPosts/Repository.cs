using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Experts.Blogger.ReadPosts;


public class Repository(Common.Solutions.Data.MainDB.MainDB db) : Story.IRepository {
    public async Task<IEnumerable<Common.Business.Model.Post>> Read(Story.Request request, CancellationToken token) {
        var solutionModel = await db
            .Posts
            .Include(x => x.PostTags)
            .ThenInclude(x => x.Tag)
            .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
            .ToListAsync(token);

        var businsessModel = solutionModel
            .Select(model => new Common.Business.Model.Post() {
                Title = model.Title,
                Content = model.Content
            });

        return businsessModel;
    }
}
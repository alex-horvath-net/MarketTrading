using Common.Business.Model;
using Common.Solutions.Data.MainDB;
using Experts.Blogger.ReadPosts.Business;
using Experts.Blogger.ReadPosts.Business.Model;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Experts.Blogger.ReadPosts.Solutions;

public class Repository(MainDB db) : IRepository {
    public async Task<IEnumerable<Post>> Read(Request request, CancellationToken token) {
        var solutionModel = await db.Posts
            .Include(post => post.PostTags)
            .ThenInclude(postTag => postTag.Tag)
            .Where(post => 
                request.Filter == null || 
                post.Title.Contains(request.Filter) || 
                post.Content.Contains(request.Filter) ||
                post.PostTags.Any(pt=>pt.Tag.Name.Contains(request.Filter)))
            .ToListAsync(token);

        var businsessModel = solutionModel
            .Select(model => new Post() {
                Id = model.PostId,
                Title = model.Title,
                Content = model.Content,
                CreatedAt = model.CreatedAt,
                Tags = model.PostTags.Select(pt=> new Tag(pt.Tag.TagId, pt.Tag.Name)).ToList(),
            });

        return businsessModel;
    }
}


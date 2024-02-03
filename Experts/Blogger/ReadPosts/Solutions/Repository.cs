using Common.Business.Model;
using Common.Solutions.Data.MainDB;
using Experts.Blogger.ReadPosts.Business.Model;
using Experts.Blogger.ReadPosts.Business.SolutionPorts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Experts.Blogger.ReadPosts.Solutions;

public class Repository(MainDB db) : IRepository {
  public async Task<IEnumerable<Post>> Read(Request request, CancellationToken token) {
    var solutionModel = await db
      .Posts
      .Include(post => post.PostTags)
      .ThenInclude(postTag => postTag.Tag)
      .Where(post => post.Title.Contains(request.Title) || post.Content.Contains(request.Content))
      .ToListAsync(token);

    var businsessModel = solutionModel
      .Select(model => new Post() {
        Title = model.Title,
        Content = model.Content
      });

    return businsessModel;
  }
}


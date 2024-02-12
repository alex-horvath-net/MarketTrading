using Common.Solutions.View.Model;
using Experts.Blogger.ReadPosts.Business;
using Experts.Blogger.ReadPosts.Business.Model;

namespace Experts.Blogger.ReadPosts.Solutions;
public class Presenter : IPresenter {
    public IEnumerable<Post> ViewModel { get; set; } = [];
    public void Map(Response response) => ViewModel = response.Posts!.Select(p => new Post() {
        Id = p.Id,
        Title = p.Title,
        Content = p.Content,
        CreatedAt = p.CreatedAt,
        Tags = p.Tags.Select(t => new Tag() {
            Id = t.Id,
            Name = t.Name
        }).ToList()
    });
}

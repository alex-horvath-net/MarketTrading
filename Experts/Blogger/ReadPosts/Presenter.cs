using Common.Solutions.View.Model;

namespace Experts.Blogger.ReadPosts;
public interface IPresenter : Core.Business.IPresenter<Request, Response> { }

public class Presenter : IPresenter {
    public IEnumerable<Post> ViewModel { get; private set; }
    public void Handle(Response response) {
        var businessModel = response.Posts;

        var solutionModel = businessModel!.Select(p => new Post() {
            Id = p.Id,
            Title = p.Title,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            Tags = p.Tags.Select(t => new Tag() {
                Id = t.Id,
                Name = t.Name
            })
        });

        ViewModel = solutionModel;
    }
}
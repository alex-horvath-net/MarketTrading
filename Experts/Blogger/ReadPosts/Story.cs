using Common.Business.Model;
using Core.Business;

namespace Experts.Blogger.ReadPosts;

public class Story(
    IValidation<Request> validator, 
    IRepository repository) : StoryCore<Request, Response>(validator) {

    public override async Task RunCore(Response response, CancellationToken token) {
        response.Posts = await repository.Read(response.Request, token);
    }
}


public record Response() : ResponseCore<Request> {
    public IEnumerable<Post>? Posts { get; set; }
}


public record Request(string Title, string Content) : RequestCore;


public interface IRepository {
    Task<IEnumerable<Post>> Read(Request Request, CancellationToken token);
}

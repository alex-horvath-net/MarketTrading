using Common.Strory.Model;
using Core.Story;
using Core.Story.Model;

namespace Experts.Blogger.ReadPosts;

public class Strory(IEnumerable<IProblem<Request, Response>> tasks) : Story<Request, Response>(tasks) {
}


public record Request(string Title, string Content) : Core.Story.Model.Request {
    public static Request Empty()  => new(default, default);
}


public record Response() : Response<Request> {
    public static Response Empty() => new();
    public IEnumerable<Post>? Posts { get; set; }
}

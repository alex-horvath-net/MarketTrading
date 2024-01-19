using Common.ExpertStrory.StoryModel;
using Core.Story;
using Core.Story.StoryModel;

namespace Experts.Blogger.ReadPosts;

public class Strory(IEnumerable<IProblem<Request, Response>> tasks) : Story<Request, Response>(tasks) {
}


public record Request(string Title, string Content) : Core.Story.StoryModel.Request {
    public static readonly Request Empty  = new(default, default);
}


public record Response() : Response<Request> {
    public static readonly Response Empty = new();
    public IEnumerable<Post>? Posts { get; set; }
}

using Common.ExpertStrory.StoryModel;
using Core.ExpertStory;
using Core.ExpertStory.StoryModel;

namespace Experts.Blogger.ReadPosts;

public class Strory(IEnumerable<IProblem<Request, Response>> tasks) : Story<Request, Response>(tasks) {
}


public record Request(string Title, string Content) : Core.ExpertStory.StoryModel.Request {
    public static readonly Request Empty  = new(default, default);
}


public record Response() : Response<Request> {
    public static readonly Response Empty = new();
    public IEnumerable<Post>? Posts { get; set; }
}

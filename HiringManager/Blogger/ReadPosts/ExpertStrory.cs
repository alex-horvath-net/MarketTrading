using Common.ExpertStrory.StoryModel;
using Core.ExpertStory;
using Core.ExpertStory.StoryModel;

namespace Experts.Blogger.ReadPosts;

public class ExpertStrory(IEnumerable<IProblem<Request, Response>> tasks) : ExpertStory<Request, Response>(tasks)
{
}


public record Request(string Title, string Content) : Core.ExpertStory.StoryModel.Request {
    public static Request Empty { get; } = new(default, default);
}


public record Response() : Response<Request> {
    public static Response Empty { get; } = new();
    public IEnumerable<Post>? Posts { get; set; }
}

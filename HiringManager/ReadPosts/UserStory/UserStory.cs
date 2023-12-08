namespace Blogger.ReadPosts.UserStory;

public class UserStory(IEnumerable<Sys.UserStory.ITask<Response>> workSteps) : Sys.UserStory.UserStoryCore<Request, Response>(workSteps);

public record Request(string Title, string Content) : Sys.UserStory.RequestCore();

public record Response() : Sys.UserStory.ResponseCore<Request>()
{
    public List<App.UserStory.Post>? Posts { get; set; }
}
using FluentAssertions;
using NSubstitute;
using Sys.UserStory;
using Xunit;

namespace Blogger.ReadPosts.UserStory;

public class UserStory(IEnumerable<ITask> workSteps) : Sys.UserStory.UserStoryCore<Request, Response>(workSteps);

public record Request(string Title, string Content) : Sys.UserStory.RequestCore();

public record Response() : Sys.UserStory.ResponseCore<Request>()
{
    public List<App.UserStory.Post>? Posts { get; set; }
}

public interface ITask : Sys.UserStory.ITask<Response>;

//--Specification--------------------------------------------------

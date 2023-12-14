using Common.UserStory;

namespace Blogger.ReadPosts;

public record Response() : Core.UserStory.ResponseCore<Request>
{
    public List<Post>? Posts { get; set; }
}

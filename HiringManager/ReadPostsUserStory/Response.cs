using Common.UserStory;
using Core.UserStory;

namespace BloggerUserRole.ReadPostsUserStory;

public record Response() : ResponseCore<Request>
{
    public List<Post>? Posts { get; set; }
}

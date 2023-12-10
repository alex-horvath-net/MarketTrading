using Common.UserStory.UserStoryUnit;
using Core.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.UserStory.UserStoryUnit;

public record Response() : ResponseCore<Request>
{
    public List<Post>? Posts { get; set; }
}

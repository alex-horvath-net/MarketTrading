using Common.UserStory.DomainModel;

namespace Blogger.UserStories.ReadPosts;

public record Response() : Core.UserStory.ResponseCore<Request>
{
    public List<Post>? Posts { get; set; }
}

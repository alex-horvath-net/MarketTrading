using Core.Application.UserStory.DomainModel;
using Core.Enterprise.UserStory;

namespace Users.Blogger.UserStories.ReadPosts;

public record Response() : ResponseCore<Request>
{
    public List<Post>? Posts { get; set; }
}

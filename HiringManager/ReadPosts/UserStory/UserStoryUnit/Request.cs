using Core.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.UserStory.UserStoryUnit;

public record Request(string Title, string Content) : RequestCore
{
}
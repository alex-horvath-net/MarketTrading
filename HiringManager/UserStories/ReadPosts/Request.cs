namespace Blogger.UserStories.ReadPosts;

public record Request(
    string Title,
    string Content) : Core.UserStory.RequestCore
{
}
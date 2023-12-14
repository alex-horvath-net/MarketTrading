namespace Blogger.ReadPosts;

public record Request(
    string Title,
    string Content) : Core.UserStory.RequestCore
{
}
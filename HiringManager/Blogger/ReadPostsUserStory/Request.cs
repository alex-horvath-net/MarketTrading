namespace Users.Blogger.ReadPostsUserStory;

public record Request(
    string Title,
    string Content) : RequestCore
{
}
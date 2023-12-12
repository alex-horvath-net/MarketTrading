using Core.UserStory;

namespace BloggerUserRole.ReadPostsUserStory;

public record Request(
    string Title, 
    string Content) : RequestCore
{
}
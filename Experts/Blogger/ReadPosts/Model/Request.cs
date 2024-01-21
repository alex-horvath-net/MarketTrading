namespace Experts.Blogger.ReadPosts.Model;

public record Request(string Title, string Content) : Story.Model.Request
{
    public static Request Empty() => new(default, default);
}

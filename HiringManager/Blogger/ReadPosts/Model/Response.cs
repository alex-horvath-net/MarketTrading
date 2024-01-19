using Story.Model;

namespace Experts.Blogger.ReadPosts.Model;

public record Response() : Response<Request>
{
    public static Response Empty() => new();
    public IEnumerable<Post>? Posts { get; set; }
}

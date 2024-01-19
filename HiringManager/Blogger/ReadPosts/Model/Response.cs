using Core.Story.Model;
using Story.Problem.Model;

namespace Experts.Blogger.ReadPosts.Model;

public record Response() : Response<Request>
{
    public static Response Empty() => new();
    public IEnumerable<Post>? Posts { get; set; }
}

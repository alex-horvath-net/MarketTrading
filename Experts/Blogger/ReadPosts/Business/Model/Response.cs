using Common.Business.Model;
using Core.Business.Model;

namespace Experts.Blogger.ReadPosts.Business.Model;
public record Response() : ResponseCore<Request>() {
    public IEnumerable<Post>? Posts { get; set; } = [];
}

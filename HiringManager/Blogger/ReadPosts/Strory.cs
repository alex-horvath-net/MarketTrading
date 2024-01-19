using Experts.Blogger.ReadPosts.Model;
using Story;

namespace Experts.Blogger.ReadPosts;

public class Strory(IEnumerable<IProblem<Request, Response>> problems) : Story<Request, Response>(problems) {
}

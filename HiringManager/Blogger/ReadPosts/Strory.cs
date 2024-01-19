using Core.Story;
using Experts.Blogger.ReadPosts.Model;

namespace Experts.Blogger.ReadPosts;

public class Strory(IEnumerable<IProblem<Request, Response>> problems) : Story<Request, Response>(problems) {
}

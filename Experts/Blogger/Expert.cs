using Common;
using Experts.Blogger.ReadPosts;

namespace Experts.Blogger;

public record Expert(
    Story<Request, Response> ReadPosts
    );


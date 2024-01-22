
using Common.Business;

namespace Experts.Blogger;

public record Expert(
    Story<ReadPosts.Story.Request, ReadPosts.Story.Response> ReadPosts
    );


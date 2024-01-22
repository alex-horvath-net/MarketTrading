
using Core.Business;

namespace Experts.Blogger;

public record Expert(
     StoryCore<ReadPosts.Request, ReadPosts.Response, ReadPosts.Story> ReadPosts
    );

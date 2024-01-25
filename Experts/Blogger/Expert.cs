
using Core.Business;

namespace Experts.Blogger;

public record Expert(
     IStory<ReadPosts.Request, ReadPosts.Response, ReadPosts.Story> ReadPosts
    );

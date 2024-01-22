
namespace Experts.Blogger;

public record Expert(
    Common.Story<ReadPosts.Story.Request, ReadPosts.Story.Response> ReadPosts
    );


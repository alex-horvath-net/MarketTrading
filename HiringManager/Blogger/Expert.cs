using Story;

namespace Experts.Blogger;

public record Expert(
    Story<ReadPosts.Model.Request, ReadPosts.Model.Response> ReadPosts
    );


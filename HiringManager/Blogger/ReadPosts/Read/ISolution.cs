using Common.ExpertStrory.StoryModel;

namespace Experts.Blogger.ReadPosts.Read;

public interface ISolution {
    Task<IEnumerable<Post>> Read(Request Request, CancellationToken token);
}

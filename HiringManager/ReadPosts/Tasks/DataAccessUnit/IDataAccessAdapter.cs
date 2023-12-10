using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Common.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.Tasks.DataAccessUnit;

public interface IDataAccessAdapter
{
    Task<List<Post>> Read(Request request, CancellationToken cancellation);
}

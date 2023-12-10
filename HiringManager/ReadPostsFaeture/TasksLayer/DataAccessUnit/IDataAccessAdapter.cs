using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Common.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.TasksLayer.DataAccessUnit;

public interface IDataAccessAdapter
{
    Task<List<Post>> Read(Request request, CancellationToken cancellation);
}

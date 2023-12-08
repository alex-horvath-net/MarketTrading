
namespace Blogger.ReadPosts.Tasks;

public class AddPosts(IDataAccess dataAccess) : Sys.UserStory.ITask<UserStory.Response>
{
    public async Task Run(UserStory.Response response, CancellationToken cancellation)
    {
        response.Posts = await dataAccess.Read(response.Request, cancellation);
    }
}

public interface IDataAccess
{
    Task<List<App.UserStory.Post>> Read(UserStory.Request request, CancellationToken cancellation);
}

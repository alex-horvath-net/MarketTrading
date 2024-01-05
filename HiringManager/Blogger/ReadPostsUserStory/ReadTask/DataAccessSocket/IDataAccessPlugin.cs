namespace Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

public interface IDataAccessPlugin
{
    Task<List<DataModel.Post>> Read(string title, string content, CancellationToken token);
}

namespace Users.Blogger.ReadPostsUserStory.ReadTask;

public interface IDataAccessSocket
{
    Task<List<DomainModel.Post>> Read(Request request, CancellationToken token);
}

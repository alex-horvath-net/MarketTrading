namespace Users.Blogger.ReadPostsUserStory.ReadTask.DataAccessSocket;

public class Socket(IDataAccessPlugin plugin) : IDataAccessSocket
{
    public async Task<List<DomainModel.Post>> Read(Request request, CancellationToken token)
    {
        var dataModel = await plugin.Read(request.Title, request.Content, token);
        var userStoryDomainModel = dataModel.Select(x => new DomainModel.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return userStoryDomainModel;
    }
}

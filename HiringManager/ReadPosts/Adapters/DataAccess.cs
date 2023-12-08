namespace Blogger.ReadPosts.Adapters;

public class DataAccess(IDataAccess dataAccessPlugin) : Tasks.IDataAccess
{
    public async Task<List<App.UserStory.Post>> Read(UserStory.Request request, CancellationToken token)
    {
        var adapter = await dataAccessPlugin.Read(request.Title, request.Content, token);
        var business = adapter.Select(x => new App.UserStory.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return business;
    }
}

public interface IDataAccess
{
    Task<List<App.Adapters.Post>> Read(string title, string content, CancellationToken token);
}


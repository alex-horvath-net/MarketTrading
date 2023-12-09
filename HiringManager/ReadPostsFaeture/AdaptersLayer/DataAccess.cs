using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;
using Models.UserStoryLayer.UserStoryUnit;
using Polices.AdaptersLayer;

namespace BloggerUserRole.ReadPostsFaeture.AdaptersLayer;

public class DataAccess(IDataAccess dataAccessPlugin) : TasksLayer.IDataAccess
{
    public async Task<List<Post>> Read(Request request, CancellationToken token)
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
    Task<List<Polices.AdaptersLayer.Post>> Read(string title, string content, CancellationToken token);
}

//--Test--------------------------------------------------

using BloggerUserRole.ReadPostsFaeture.TasksLayer.DataAccessUnit;
using BloggerUserRole.ReadPostsFaeture.UserStoryLayer.UserStoryUnit;

namespace BloggerUserRole.ReadPostsFaeture.AdaptersLayer.DataAccessUnit;

public class DataAccessAdapter(IDataAccessPlugin dataAccessPlugin) : IDataAccessAdapter
{
    public async Task<List<Common.UserStoryLayer.UserStoryUnit.Post>> Read(Request request, CancellationToken token)
    {
        var adapter = await dataAccessPlugin.Read(request.Title, request.Content, token);
        var business = adapter.Select(x => new Common.UserStoryLayer.UserStoryUnit.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return business;
    }
}
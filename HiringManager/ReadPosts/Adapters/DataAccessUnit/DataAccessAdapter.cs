using Blogger.ReadPosts.Tasks.DataAccessUnit;
using Blogger.ReadPosts.UserStory.UserStoryUnit;
using Common.UserStory.UserStoryUnit;

namespace Blogger.ReadPosts.Adapters.DataAccessUnit;

public class DataAccessAdapter(IDataAccessPlugin dataAccessPlugin) : IDataAccessAdapter
{
    public async Task<List<Post>> Read(Request request, CancellationToken token)
    {
        var adapter = await dataAccessPlugin.Read(request.Title, request.Content, token);
        var business = adapter.Select(x => new Common.UserStory.UserStoryUnit.Post()
        {
            Title = x.Title,
            Content = x.Content
        }).ToList();
        return business;
    }
}
using Common.Adapters.DataAccessUnit;

namespace Blogger.ReadPosts.Adapters.DataAccessUnit;

public interface IDataAccessPlugin
{
    Task<List<Post>> Read(string title, string content, CancellationToken token);
}

//--Test--------------------------------------------------

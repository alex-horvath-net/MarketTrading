using Common.AdapterLayer.DataAccessUnit;

namespace BloggerUserRole.ReadPostsFaeture.AdapterLayer.DataAccessUnit;

public interface IDataAccessPlugin
{
    Task<List<Post>> Read(string title, string content, CancellationToken token);
}

//--Test--------------------------------------------------

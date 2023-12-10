namespace BloggerUserRole.ReadPostsFaeture.AdaptersLayer.DataAccessUnit;

public interface IDataAccessPlugin
{
    Task<List<Common.AdaptersLayer.DataAccessUnit.Post>> Read(string title, string content, CancellationToken token);
}

//--Test--------------------------------------------------

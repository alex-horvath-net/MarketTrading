using Core.App.Sockets.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DBProvider
{
    public DB GetTestDB(bool delete = false)
    {
        var options = new DbContextOptionsBuilder().Dev().Options;
        var db = new DB(options);
        db.Schema(delete);

        db.Data(new Tag(Id: 1, Name: "Tag1"),
                new Tag(Id: 2, Name: "Tag2"),
                new Tag(Id: 3, Name: "Tag3"));
        
        db.Data(new Post { Id = 1, Title = "Title1", Content = "Content1", CreatedAt = DateTime.Parse("2023-12-01") },
                new Post { Id = 2, Title = "Title2", Content = "Content2", CreatedAt = DateTime.Parse("2023-12-02") },
                new Post { Id = 3, Title = "Title3", Content = "Content3", CreatedAt = DateTime.Parse("2023-12-03") });

        return db;
    }
}

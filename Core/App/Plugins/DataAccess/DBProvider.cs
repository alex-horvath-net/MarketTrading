using Core.App.Sockets.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DBProvider {
    public DB GetTestDB(bool delete = false) {
        var options = new DbContextOptionsBuilder().Dev().Options;
        var db = new DB(options);
        db.Schema(delete);

        db.Data(new Tag(1, "Tag1"),
                new Tag(2, "Tag2"),
                new Tag(3, "Tag3"));

        db.Data(new Post(1, "Title1", "Content1", DateTime.Parse("2023-12-01")),
                new Post(2, "Title2", "Content2", DateTime.Parse("2023-12-02")),
                new Post(3, "Title3", "Content3", DateTime.Parse("2023-12-03")));

        return db;
    }
}

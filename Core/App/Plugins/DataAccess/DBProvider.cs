using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DBProvider
{
    public DB GetTestDB(bool delet = false)
    {
        
        var builder = new DbContextOptionsBuilder();
        builder.Dev();

        var db = new DB(builder.Options);
        
        if (delet)
            db.Database.EnsureDeleted();

        db.Database.EnsureCreated();
        db.Database.Migrate();
        db.Database.Seed();
        return db;
    }
}

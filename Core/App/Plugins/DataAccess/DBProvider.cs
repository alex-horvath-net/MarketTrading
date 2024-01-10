using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DBProvider
{
    public DB GetTestDB(bool delet = false)
    {
        var options = new DbContextOptionsBuilder().Dev().Options;
        var db = new DB(options).Scrach(delet);

        return db;
    }
}

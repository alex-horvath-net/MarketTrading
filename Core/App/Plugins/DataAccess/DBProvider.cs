using System;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DBProvider
{
    public DB GetTestDB(bool delet = false)
    {
        
        var builder = new DbContextOptionsBuilder();
        builder.Dev();

        var db = new DB(builder.Options);

        db.Scrach(delet);

        return db;
    }
}

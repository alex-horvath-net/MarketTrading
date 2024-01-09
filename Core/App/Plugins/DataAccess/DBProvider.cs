using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.App.Plugins.DataAccess;

public class DBProvider
{
    public DB GetTestDB()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DB>()
            .EnableDetailedErrors()
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug().AddConsole().SetMinimumLevel(LogLevel.Debug)))
            .EnableSensitiveDataLogging()
            .UseSqlite<DB>(GetConnectionString(), options => options.CommandTimeout(60));

        var db = new DB(optionsBuilder.Options);
        //db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.Database.Migrate();
        db.Database.Seed();
        return db;
    }

    private string GetConnectionString()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, $"TestDatabase.db");
        var connectionString = $"Data Source={dbPath}";
        return connectionString;
    }
}

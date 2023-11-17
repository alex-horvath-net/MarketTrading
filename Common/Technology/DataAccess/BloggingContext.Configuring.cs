using Microsoft.EntityFrameworkCore;

namespace Shared.Technology.DataAccess;

public partial class BloggingContext
{
    public BloggingContext(DbContextOptions<BloggingContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, "Blogging.db");
        options.UseSqlite($"Data Source={dbPath}");

        //options.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={"Blogging"};Trusted_Connection=True");
    }
}

using Core.App.Sockets.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DB(DbContextOptions options) : DbContext(options) {
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
}

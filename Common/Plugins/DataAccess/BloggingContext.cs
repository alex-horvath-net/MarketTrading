using Core.PluginAdapters.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Shared.Technology.DataAccess;

public partial class BloggingContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
}

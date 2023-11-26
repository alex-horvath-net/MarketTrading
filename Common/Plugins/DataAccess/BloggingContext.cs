using Core.PluginAdapters;
using Microsoft.EntityFrameworkCore;

namespace Core.Technology.DataAccess;

public partial class BloggingContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
}

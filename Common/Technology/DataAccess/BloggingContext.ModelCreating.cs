using Microsoft.EntityFrameworkCore;
using Shared.Adapter.DataModel;

namespace Shared.Technology.DataAccess;

public partial class BloggingContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().ToTable("Post");
        modelBuilder.Entity<Tag>().ToTable("Tag");
    }
}

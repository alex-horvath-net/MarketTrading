using Microsoft.EntityFrameworkCore;
using Shared.Adapter.DataModel;

namespace Shared.Technology.Data
{
    public class BloggingContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public BloggingContext(DbContextOptions<BloggingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Tag>().ToTable("Tag");
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
}

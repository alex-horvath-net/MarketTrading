using Common.Adapters.DataAccessUnit;
using Microsoft.EntityFrameworkCore;

namespace Assistant.Plugins;

public partial class BlogDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
}

public partial class BlogDbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var databaseName = "Blogging";
        var dbPath = Path.Join(path, $"{databaseName}.db");
        options.UseInMemoryDatabase(databaseName, x => x.EnableNullChecks(true));
        //options.UseSqlite($"Data Source={dbPath}");

        //options.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={"Blogging"};Trusted_Connection=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().ToTable("Post");
        modelBuilder.Entity<Tag>().ToTable("Tag");
    }

    public void EnsureInitialized()
    {
        var seeded = Tags.Any() || Posts.Any();
        if (seeded) return;

        InitializeTags();
        InitializePosts();

        void InitializeTags()
        {
            var tags = new Tag[]
            {
                new Tag(Id:1,Name:"Tag1" ),
                new Tag(Id:2,Name:"Tag2" ),
                new Tag(Id:3,Name:"Tag3" ),
            };
            Tags.AddRange(tags);
            SaveChanges();
        }

        void InitializePosts()
        {
            var posts = new Post[]
            {
                new Post{ Id=1, Title="Title1",Content="Content1",CreatedAt=DateTime.Parse("2023-12-01")},
                new Post{ Id=2, Title="Title2",Content="Content2",CreatedAt=DateTime.Parse("2023-12-02")},
                new Post{ Id=3, Title="Title3",Content="Content3",CreatedAt=DateTime.Parse("2023-12-03")}
            };
            Posts.AddRange(posts);
            SaveChanges();
        }
    }
}

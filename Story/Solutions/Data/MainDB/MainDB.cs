using Common.Solutions.Data.MainDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Common.Solutions.Data.MainDB;

public class MainDB : DbContext {
  private readonly string connectionString;

  public MainDB(DbContextOptions<MainDB> options, IOptions<Common> settings) : base(options) {
    this.connectionString = settings.Value.Solutions.Data.MainDB.ConnectionString;
  }
  //public MainDB(DbContextOptions<MainDB> options, IOptions<Solutions> settings) : DbContext(options) { }


  //public MainDB() : this(new DbContextOptionsBuilder().Dev().Options) { } 

  public DbSet<Post> Posts { get; set; }
  public DbSet<Tag> Tags { get; set; }
  public DbSet<PostTag> PostTags { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    if (optionsBuilder.IsConfigured)
      return;

    optionsBuilder
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging()
        .UseSqlServer(connectionString, sqlOptionsBuilder => {
          //.UseSqlServer($"name = ConnectionStrings:{nameof(MainDB)}", sqlOptionsBuilder => {
          sqlOptionsBuilder.CommandTimeout(60);
        });


    base.OnConfiguring(optionsBuilder);
  }

  protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);

    builder.Entity<PostTag>(entity => {
      entity.HasKey(pt => new { pt.PostId, pt.TagId });

      entity.HasOne(pt => pt.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId);

      entity.HasOne(pt => pt.Tag)
            .WithMany(t => t.PostTags)
            .HasForeignKey(pt => pt.TagId);
    });

    builder.Entity<Post>().HasData(
        new(1, "Title1", "Content1", DateTime.Parse("2023-12-01")),
        new(2, "Title2", "Content2", DateTime.Parse("2023-12-02")),
        new(3, "Title3", "Content3", DateTime.Parse("2023-12-03")));

    builder.Entity<Tag>().HasData(
        new(1, "Tag1"),
        new(2, "Tag2"));

    builder.Entity<PostTag>().HasData(
        new(1, 1),
        new(1, 2),
        new(2, 1));
  }
}

/*
 * dotnet tool install --global dotnet-ef
 * dotnet tool update --global dotnet-ef
 * dotnet add package Microsoft.EntityFrameworkCore.Design
 * dotnet ef
 * Add-Migration InitialCreate --startup-project WebSite --Project Story --context DevDB --OutputDir Story\Solutions\Data\MainDB\Migrations
 * 
 * 
 * 
 * Install tool:
 *      dotnet tool install --global dotnet-ef
 * 
 * Update tool:
 *      dotnet tool update --global dotnet-ef
 * 
 * On Migration project
 *      dotnet add Core package Microsoft.EntityFrameworkCore.Design
 *      
 * dotnet ef migrations add InitialCreate --output-dir \App\Plugins\DataAccess\Migrations
 * 
 * VisualStudio Package Manager Console
 *      Installing the tools:       Install-Package Microsoft.EntityFrameworkCore.Tools
 *      Update the tools:           Update-Package Microsoft.EntityFrameworkCore.Tools
 *      Verify the installation:    Get-Help about_EntityFrameworkCore
 *      Add-Migration InitialCreate -startupproject WebSite -Project Story -context MainDB -OutputDir Solutions\Data\MainDB\Migrations
 *      Update-Database -startupproject WebAppMvc -Project Common -context MainDB -Args '--environment Development'
 */



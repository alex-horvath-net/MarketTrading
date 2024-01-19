using Common.Solutions.Data.MainDB.Model;
using Microsoft.EntityFrameworkCore;

namespace Common.Solutions.Data.MainDB;

public class MainDB(DbContextOptions options) : DbContext(options)
{
    public MainDB() : this(new DbContextOptionsBuilder().Dev().Options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<PostTag>()
            .HasKey(pt => new { pt.PostId, pt.TagId });

        builder
            .Entity<PostTag>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId);

        builder
            .Entity<PostTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PostTags)
            .HasForeignKey(pt => pt.TagId);

        var post1 = new Post(1, "Title1", "Content1", DateTime.Parse("2023-12-01"));
        var post2 = new Post(2, "Title2", "Content2", DateTime.Parse("2023-12-02"));
        var post3 = new Post(3, "Title3", "Content3", DateTime.Parse("2023-12-03"));

        var tag1 = new Tag(1, "Tag1");
        var tag2 = new Tag(2, "Tag2");

        var postTag1 = new PostTag(1, 1);
        var postTag2 = new PostTag(1, 2);
        var postTag3 = new PostTag(2, 1);

        builder.Entity<Post>().HasData(post1, post2, post3);
        builder.Entity<Tag>().HasData(tag1, tag2);
        builder.Entity<PostTag>().HasData(postTag1, postTag2, postTag3);
    }
}

/*
 * dotnet tool install --global dotnet-ef
 * dotnet tool update --global dotnet-ef
 * dotnet add package Microsoft.EntityFrameworkCore.Design
 * dotnet ef
 * Add-Migration InitialCreate --startup-project WebSite --Project Core --context DB --OutputDir \App\Plugins\DataAccess\Migrations
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
 *      ?command line arguments:    Update-Database -Args '--environment Production'
 */



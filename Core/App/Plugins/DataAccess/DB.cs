using Core.App.Sockets.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DB(DbContextOptions options) : DbContext(options) {
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        
        builder.Entity<Post>().HasMany(p => p.Tags).WithMany(t => t.Posts);
        
        builder.Entity<Post>().HasData(
            new Post(1, "Title1", "Content1", DateTime.Parse("2023-12-01")),
            new Post(2, "Title2", "Content2", DateTime.Parse("2023-12-02")),
            new Post(3, "Title3", "Content3", DateTime.Parse("2023-12-03")));

        builder.Entity<Tag>().HasData(
            new Tag(1, "Tag1"),
            new Tag(2, "Tag2"));
    }
}
/*
 * dotnet tool install --global dotnet-ef
 * dotnet tool update --global dotnet-ef
 * dotnet add package Microsoft.EntityFrameworkCore.Design
 * dotnet ef
 * Add-Migration InitialCreate --startup-project WebApp --Project Core --context DB --OutputDir \App\Plugins\DataAccess\Migrations
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



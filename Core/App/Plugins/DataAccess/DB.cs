using Core.App.Sockets.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Core.App.Plugins.DataAccess;

public class DB(DbContextOptions options) : DbContext(options) {
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
}
/*
 * dotnet tool install --global dotnet-ef
 * dotnet tool update --global dotnet-ef
 * dotnet add package Microsoft.EntityFrameworkCore.Design
 * dotnet ef
 * Add-Migration InitialCreate --startup-project WenApp--Project Core --context DB --OutputDir \App\Plugins\DataAccess\Migrations
 * 
 * dotnet ef migrations add InitialCreate --output-dir \App\Plugins\DataAccess\Migrations
 */



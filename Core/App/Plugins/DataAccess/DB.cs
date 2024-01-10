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



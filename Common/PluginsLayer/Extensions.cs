using Assistant.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Polices.PluginsLayer;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseName = "Blogging";
        var connectionString = configuration.GetConnectionString(databaseName);
        services.AddDbContext<BloggingContext>(options => options.UseInMemoryDatabase(databaseName));
        //builder.Services.AddDbContext<BloggingContext>(options => options.UseSqlite(connectionString));
        //builder.Services.AddDbContext<BloggingContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    public static WebApplication UseDataBase(this WebApplication app)
    {
        app.UseMigrationsEndPoint();

        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<BloggingContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.EnsureInitialized();

        return app;
    }
}

using Assistant.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.PluginLayer.DataAccessUnit;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseName = "Blogging";
        var connectionString = configuration.GetConnectionString(databaseName);
        services.AddDbContext<BlogDbContext>(options => options.UseInMemoryDatabase(databaseName));
        //builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlite(connectionString));
        //builder.Services.AddDbContext<BlogDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    public static WebApplication UseDataBase(this WebApplication app)
    {
        app.UseMigrationsEndPoint();

        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.EnsureInitialized();

        return app;
    }
}

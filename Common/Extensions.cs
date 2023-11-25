using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Technology.DataAccess;

namespace Core;

public static class Extensions
{
    public static Task<T> ToTask<T>(this T value) => Task.FromResult(value);

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

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var db = services.GetRequiredService<BloggingContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.EnsureInitialized();
        }

        return app;
    }
}

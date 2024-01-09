using Core.App.Sockets.DataModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.App.Plugins.DataAccess;

public static class DBExtensions
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration, bool isDev = false)
    {
        services.AddDbContext<DB>(builder =>
            {
                if (isDev)
                    builder.Dev();
                else
                    builder.Prod();
            });

        return services;
    }

    public static void Dev(this DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .EnableDetailedErrors()
        .UseLoggerFactory(LoggerFactory.Create(logBuilder => logBuilder.AddDebug().AddConsole().SetMinimumLevel(LogLevel.Debug)))
        .EnableSensitiveDataLogging()
        .UseSqlite("Data Source=TestDatabase.db", sqliteBuilder => sqliteBuilder.CommandTimeout(60));

    public static void Prod(this DbContextOptionsBuilder optionsBuilder) => optionsBuilder
        .EnableDetailedErrors()
        .UseLoggerFactory(LoggerFactory.Create(logBuilder => logBuilder.AddDebug().AddConsole().SetMinimumLevel(LogLevel.Debug)))
        .EnableSensitiveDataLogging()
        .UseSqlite("Data Source=ProdDatabase.db", sqliteBuilder => sqliteBuilder.CommandTimeout(60));

    public static WebApplication UseDataBase(this WebApplication app)
    {
        app.UseMigrationsEndPoint();

        using var scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<DB>();

        //db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        db.Database.Migrate();
        db.Database.Seed();

        return app;
    }

    public static void Seed(this DatabaseFacade databaseFacade)
    {
        var db = (DB)((IDatabaseFacadeDependenciesAccessor)databaseFacade).Context;

        if (!db.Tags.Any())
        {
            var tags = new Tag[]
            {
                new Tag(Id:1,Name:"Tag1" ),
                new Tag(Id:2,Name:"Tag2" ),
                new Tag(Id:3,Name:"Tag3" ),
            };
            db.Tags.AddRange(tags);
            db.SaveChanges();
        }

        if (!db.Posts.Any())
        {
            var posts = new Post[]
            {
                new Post{ Id=1, Title="Title1",Content="Content1",CreatedAt=DateTime.Parse("2023-12-01")},
                new Post{ Id=2, Title="Title2",Content="Content2",CreatedAt=DateTime.Parse("2023-12-02")},
                new Post{ Id=3, Title="Title3",Content="Content3",CreatedAt=DateTime.Parse("2023-12-03")}
            };

            db.Posts.AddRange(posts);
            db.SaveChanges();
        }
    }
}

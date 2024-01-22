using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.Solutions.Data.MainDB;

public static class Extensions {
    public static IServiceCollection AddCommonSolutions(this IServiceCollection services, IConfiguration configuration) => services
        .AddMainDB(configuration);

        public static IServiceCollection AddMainDB(this IServiceCollection services, IConfiguration configuration, string environment = "Development") {
        services.AddDbContext<MainDB>(builder => {
            if (environment == Environments.Development)
                builder.Dev(configuration);
            else
                builder.Prod(configuration);
        });
        return services;
    }

    public static DbContextOptionsBuilder Dev(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration) => optionsBuilder
        .EnableDetailedErrors()
        .UseLoggerFactory(LoggerFactory.Create(logBuilder => logBuilder.AddConfiguration(configuration)))
        .EnableSensitiveDataLogging()
        .UseSqlServer(@"Data Source=.;Initial Catalog=MainDB-Dev;Integrated Security=True;Trust Server Certificate=True", sqliteBuilder => sqliteBuilder.CommandTimeout(60));
     
    public static DbContextOptionsBuilder Prod(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration) => optionsBuilder
        .EnableDetailedErrors()
        .UseLoggerFactory(LoggerFactory.Create(logBuilder => logBuilder.AddConfiguration(configuration)))
        .EnableSensitiveDataLogging()
        .UseSqlServer(@"Server=.;Database=MainDB-Prod;Trusted_Connection=True;", sqliteBuilder => sqliteBuilder.CommandTimeout(60));

    public static MainDB UseDeveloperDataBase(this WebApplication app, bool delete = false) {
        //app.UseMigrationsEndPoint();

        var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<MainDB>();

        db.Schema(delete);

        //db.Data(new Tag(1, "Tag1"),
        //        new Tag(2, "Tag2"),
        //        new Tag(3, "Tag3"));

        //db.Data(new Post(1, "Title1", "Content1", DateTime.Parse("2023-12-01")),
        //        new Post(2, "Title2", "Content2", DateTime.Parse("2023-12-02")),
        //        new Post(3, "Title3", "Content3", DateTime.Parse("2023-12-03")));

        return db;
    }

    public static MainDB Schema(this MainDB db, bool delete = false) {
        if (delete)
            db.Database.EnsureDeleted();
        //db.Database.EnsureCreated();
        db.Database.Migrate();

        return db;
    }
    public static void Data<T>(this MainDB db, params T[] list) where T : class => db.Data(list);
    public static MainDB Data<T>(this MainDB db, IEnumerable<T> list) where T : class {
        var set = db.Set<T>();
        if (!set.Any()) {
            set.AddRange(list);
            db.SaveChanges();
        }
        return db;
    }
}

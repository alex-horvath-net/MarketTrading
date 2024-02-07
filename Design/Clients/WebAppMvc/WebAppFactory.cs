using Clients.WebAppMvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAppMvc.Controllers;

namespace Clients.WebAppMvc {
    public class WebAppFactory : WebApplicationFactory<PostController> {
        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            builder.ConfigureServices(services => {
                // Here you can set up test services or replace production services with test versions
                // Example:
                // var serviceProvider = new ServiceCollection()
                //     .AddEntityFrameworkInMemoryDatabase()
                //     .BuildServiceProvider();

                // services.AddDbContext<AppDbContext>(options =>
                // {
                //     options.UseInMemoryDatabase("InMemoryDbForTesting");
                //     options.UseInternalServiceProvider(serviceProvider);
                // });

                // Remove the app's ApplicationDbContext registration
                // var descriptor = services.SingleOrDefault(
                //     d => d.ServiceType ==
                //         typeof(DbContextOptions<AppDbContext>));

                // if (descriptor != null)
                // {
                //     services.Remove(descriptor);
                // }

                // Add ApplicationDbContext using an in-memory database for testing
                // services.AddDbContext<AppDbContext>(options =>
                // {
                //     options.UseInMemoryDatabase("InMemoryDbForTesting");
                // });

                // Build the service provider
                // var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context (AppDbContext)
                // using (var scope = sp.CreateScope())
                // {
                //     var scopedServices = scope.ServiceProvider;
                //     var db = scopedServices.GetRequiredService<AppDbContext>();
                //     var logger = scopedServices
                //         .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                //     // Ensure the database is created
                //     db.Database.EnsureCreated();

                //     try
                //     {
                //         // Seed the database with test data
                //         Utilities.InitializeDbForTests(db);
                //     }
                //     catch (Exception ex)
                //     {
                //         logger.LogError(ex, "An error occurred seeding the " +
                //                             "database with test messages. Error: {Message}", ex.Message);
                //     }
                // }
            });

            builder.ConfigureAppConfiguration((context, config) => {
                // config.AddJsonFile("appsettings.Test.json");
            });

            builder.Configure(app => {
                // Example: Disable middleware not applicable to tests
                // app.UseMiddleware<SomeMiddleware>();
            });
        }
    }
}


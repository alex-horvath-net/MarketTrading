using Microsoft.EntityFrameworkCore;

namespace TradingService.Infrastructure.Database;

public static class DatabaseExtensions {
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration) {
        services.AddDbContext<TradingDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
    }
}
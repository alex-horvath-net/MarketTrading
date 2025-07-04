using Microsoft.EntityFrameworkCore;
using TradingService.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);
// 1) EF 
builder.Services.AddDbContext<TradingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<TradingDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.MapGet("/ping", () => Results.Ok($"TradingService {DateTime.Now:dd/MM/yyyy HH:mm:ss}")).AllowAnonymous();

app.MapGet("/sping", () => "secure pong").RequireAuthorization();

app.Run();

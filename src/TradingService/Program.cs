using Microsoft.EntityFrameworkCore;
using TradingService.Domain;
using TradingService.Domain.Orders.Commands;
using TradingService.Infrastructure.Database;
using TradingService.Infrastructure.Time;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AdddLondonTime();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddEventStore();
builder.Services.AddDomain();


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

app.MapPost("/command/placeorder", (PlaceOrderCommand command, CommandRouter<Guid> router) => {
    router.HandleCommand(command);
    return Results.Accepted();

}).AllowAnonymous();

app.MapGet("/ping", () => Results.Ok($"TradingService {DateTime.Now:dd/MM/yyyy HH:mm:ss}")).AllowAnonymous();

app.MapGet("/sping", () => "secure pong").RequireAuthorization();

app.Run();

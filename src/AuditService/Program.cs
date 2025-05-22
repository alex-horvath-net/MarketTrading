using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/ping", () => "pong");

app.Run();

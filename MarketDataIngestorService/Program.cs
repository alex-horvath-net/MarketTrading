using MarketDataIngestionService.Infrastructure.Host;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MarketDataIngestorBackgroundService>();

var host = builder.Build();
host.Run();

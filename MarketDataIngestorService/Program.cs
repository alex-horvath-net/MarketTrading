using MarketDataIngestorService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MarketDataIngestorServiceWorker>();

var host = builder.Build();
host.Run();

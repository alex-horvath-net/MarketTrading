using MarketDataRelayService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MarketDataRelayBackgroundService>();

var host = builder.Build();
host.Run();

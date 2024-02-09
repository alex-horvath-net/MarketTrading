using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
namespace UI.WebAppMvc;

public class WebAppFactory : WebApplicationFactory<Program> {
    //public WebAppFactory() {
    //    ClientOptions.AllowAutoRedirect = false;
    //    ClientOptions.BaseAddress = new Uri("https://localhost");
    //}

    //protected override void ConfigureWebHost(IWebHostBuilder webHostBuilder) {
    //    webHostBuilder.ConfigureAppConfiguration(configBuilder => {
    //        var dataDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    //        if (!Directory.Exists(dataDirectory)) {
    //            Directory.CreateDirectory(dataDirectory);
    //        }

    //        var config = new[]            {
    //            KeyValuePair.Create<string, string?>("DataDirectory", dataDirectory)
    //        };

    //        configBuilder.AddInMemoryCollection(config);
    //    });

    //    webHostBuilder.ConfigureLogging(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Debug).AddDebug().AddConsole());

    //    //webHostBuilder.ConfigureKestrel(serverOptions =>
    //    //    serverOptions.ConfigureHttpsDefaults(httpsOptions =>
    //    //        httpsOptions.ServerCertificate = new X509Certificate2("localhost-dev.pfx", "Pa55w0rd!")));

    //    webHostBuilder.UseUrls("http://127.0.0.1:0");
    //}

    protected override IHost CreateHost(IHostBuilder hostBuilder) {
        var testServerHost = hostBuilder.Build();

        this.kestrelServerHost = BuildAndStartKestrelServerHost(hostBuilder);
        base.ClientOptions.BaseAddress = GetKestrelServerAddress();

        return testServerHost;
    }

    private IHost BuildAndStartKestrelServerHost(IHostBuilder hostBuilder) {
        var host = hostBuilder
            .ConfigureWebHost(webHostBuilder => webHostBuilder
                .UseUrls("http://127.0.0.1:0")
                .UseKestrel())
            .Build();

        host.Start();

        return host;
    }

    private Uri GetKestrelServerAddress() {
        var baseAddraes = this.kestrelServerHost!
            .Services.GetRequiredService<IServer>()
            .Features.Get<IServerAddressesFeature>()!
            .Addresses.Last();

        return new Uri(baseAddraes);
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);

        if (!_disposed && disposing) kestrelServerHost?.Dispose();
        _disposed = true;
    }

    private bool _disposed;
    private IHost? kestrelServerHost;
}



using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
namespace UI.WebAppMvc;

public class WebAppFactory : WebApplicationFactory<Program> {
    public WebAppFactory() {
        ClientOptions.AllowAutoRedirect = false;
        ClientOptions.BaseAddress = new Uri("https://localhost");
    }

    protected override void ConfigureWebHost(IWebHostBuilder webHostBuilder) {
        webHostBuilder.ConfigureAppConfiguration(configBuilder => {
            var dataDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            if (!Directory.Exists(dataDirectory)) {
                Directory.CreateDirectory(dataDirectory);
            }

            var config = new[]            {
                KeyValuePair.Create<string, string?>("DataDirectory", dataDirectory)
            };

            configBuilder.AddInMemoryCollection(config);
        });

        webHostBuilder.ConfigureLogging(loggingBuilder => loggingBuilder.SetMinimumLevel(LogLevel.Debug).AddDebug().AddConsole());

        //webHostBuilder.ConfigureKestrel(serverOptions =>
        //    serverOptions.ConfigureHttpsDefaults(httpsOptions =>
        //        httpsOptions.ServerCertificate = new X509Certificate2("localhost-dev.pfx", "Pa55w0rd!")));

        webHostBuilder.UseUrls("http://127.0.0.1:0");
    }

    protected override IHost CreateHost(IHostBuilder hostBuilder) {
        var testServerHost = hostBuilder.Build();

        // Modify the host hostBuilder to use Kestrel instead TestServer so we can listen on a real address.
        hostBuilder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        // Create and start the Kestrel server before the test server
        _host = hostBuilder.Build();
        _host.Start();

        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses.Select(address => new Uri(address)).Last();

        // Return the host that uses TestServer, rather than the real one.
        // Otherwise the internals will complain about the host's server
        // not being an instance of the concrete type TestServer.
        return testServerHost;
    }

    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);

        if (!_disposed) {
            if (disposing) {
                _host?.Dispose();
            }

            _disposed = true;
        }
    }

    private void EnsureServer() {
        if (_host is null) {
            using var _ = base.CreateDefaultClient();
        }
    }

    private bool _disposed;
    private IHost? _host;

    public string ServerAddress {
        get {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    public override IServiceProvider Services {
        get {
            EnsureServer();
            return _host!.Services!;
        }
    }



}

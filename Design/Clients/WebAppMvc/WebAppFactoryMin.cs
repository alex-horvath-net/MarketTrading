using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAppMvc;

namespace Clients.WebAppMvc {
    public class WebAppFactoryMin : WebApplicationFactory<FileName> {
        private IHost _hostThatRunsTestServer;
        private IHost _hostThatRunsKestrelImpl;

        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            builder.UseUrls("https://localhost:7287");
            // ... other configurations
        }

        protected override IHost CreateHost(IHostBuilder builder) {
            try {
                _hostThatRunsTestServer = builder.Build();
                builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());
                _hostThatRunsKestrelImpl = builder.Build();
                _hostThatRunsKestrelImpl.Start();
                var server = _hostThatRunsKestrelImpl.Services.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>();
                ClientOptions.BaseAddress = addresses!.Addresses.Select(x => new Uri(x)).Last();
                _hostThatRunsTestServer.Start();
                return _hostThatRunsTestServer;
            }
            catch (Exception e) {
                _hostThatRunsKestrelImpl?.Dispose();
                _hostThatRunsTestServer?.Dispose();
                throw;
            }
        }
    }
}

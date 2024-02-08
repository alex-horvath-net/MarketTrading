//using Azure.Core;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;

//namespace Clients.WebAppMvc;

//public class WebAppFactory : WebApplicationFactory<Program> {
//    public WebAppFactory() {
//        ClientOptions.AllowAutoRedirect = false;
//        ClientOptions.BaseAddress = new Uri("https://localhost");
//    }

//    public ITestOutputHelper? OutputHelper { get; set; }

//    protected override void ConfigureWebHost(IWebHostBuilder builder) {
//        builder.ConfigureAppConfiguration(configBuilder => {
//            // Configure the test fixture to write the SQLite database
//            // to a temporary directory, rather than in App_Data.
//            var dataDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

//            if (!Directory.Exists(dataDirectory)) {
//                Directory.CreateDirectory(dataDirectory);
//            }

//            // Also override the default options for the GitHub OAuth provider
//            var config = new[]
//            {
//                KeyValuePair.Create<string, string?>("DataDirectory", dataDirectory),
//                KeyValuePair.Create<string, string?>("GitHub:ClientId", "github-id"),
//                KeyValuePair.Create<string, string?>("GitHub:ClientSecret", "github-secret"),
//                KeyValuePair.Create<string, string?>("GitHub:EnterpriseDomain", string.Empty)
//            };

//            configBuilder.AddInMemoryCollection(config);
//        });

//        // Route the application's logs to the xunit output
//        builder.ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders().AddXUnit(this));

//        // Configure the correct content root for the static content and Razor pages
//        builder.UseSolutionRelativeContentRoot(Path.Combine("src", "TodoApp"));

//        // Configure the application so HTTP requests related to the OAuth flow
//        // can be intercepted and redirected to not use the real GitHub service.
//        builder.ConfigureServices(services => {
//            services.AddHttpClient();

//            services.AddSingleton<IHttpMessageHandlerBuilderFilter, HttpRequestInterceptionFilter>(
//                _ => new HttpRequestInterceptionFilter(Interceptor));

//            services.AddSingleton<IPostConfigureOptions<GitHubAuthenticationOptions>, RemoteAuthorizationEventsFilter>();
//            services.AddScoped<LoopbackOAuthEvents>();
//        });

//        // Configure a bundle of HTTP requests to intercept for the OAuth flow.
//        Interceptor.RegisterBundle("oauth-http-bundle.json");
//    }
//}

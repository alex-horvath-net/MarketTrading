using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Microsoft.ReverseProxy.Abstractions;
using System.Collections.Generic;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Optional: add Console logging (or any custom logging)
builder.Logging.AddConsole();

// Register YARP and load in‑memory configuration for routes and clusters.
builder.Services.AddReverseProxy()
    .LoadFromMemory(GetRoutes(), GetClusters());

var app = builder.Build();

app.UseRouting();

// Optionally, add middleware for error handling or logging here.
// e.g. app.UseMiddleware<CustomLoggingMiddleware>();

// Map YARP reverse proxy to handle all routes matching the defined routes.
app.UseEndpoints(endpoints => {
    endpoints.MapReverseProxy();
});

app.Run();

// ------------------------------------------------------------------
// Helper Methods: Define In‑Memory Routes and Clusters for YARP
// ------------------------------------------------------------------

IReadOnlyList<RouteConfig> GetRoutes() => new List<RouteConfig>
{
    // Route for IdentityService
    new RouteConfig
    {
        RouteId = "identity_route",
        ClusterId = "identity_cluster",
        Match = new RouteMatch
        {
            Path = "/identity/{**catch-all}"
        }
    },
    // Route for OrderManagement (or any additional backend service)
    new RouteConfig
    {
        RouteId = "orders_route",
        ClusterId = "orders_cluster",
        Match = new RouteMatch
        {
            Path = "/orders/{**catch-all}"
        }
    }
};

IReadOnlyList<ClusterConfig> GetClusters() => new List<ClusterConfig>
{
    // Cluster pointing to the IdentityService container
    new ClusterConfig
    {
        ClusterId = "identity_cluster",
        Destinations = new Dictionary<string, DestinationConfig>
        {
            { "dest1", new DestinationConfig { Address = "http://yourbank_identityservice:80" } }
        }
    },
    // Cluster pointing to the OrderManagement service (as an example)
    new ClusterConfig
    {
        ClusterId = "orders_cluster",
        Destinations = new Dictionary<string, DestinationConfig>
        {
            { "dest1", new DestinationConfig { Address = "http://yourbank_ordermanagement:80" } }
        }
    }
};

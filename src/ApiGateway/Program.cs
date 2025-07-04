var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.AddDebug().AddConsole();

// Load YARP reverse proxy configuration
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Configure authentication & authorization
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options => {
        // IdentityService listens on host 5001 mapped to container HTTPS 443
        options.Authority = "https://localhost:5001";
        options.Audience = "gateway";
        options.RequireHttpsMetadata = true;
    });

builder.Services.AddAuthorization();

// Configure CORS
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Middleware pipeline
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Reverse proxy endpoints (protected by auth)
app.MapReverseProxy();

// Health check
app.MapGet("/ping", () => Results.Ok($"ApiGateway {DateTime.Now:dd/MM/yyyy HH:mm:ss}")).AllowAnonymous();

app.Run();

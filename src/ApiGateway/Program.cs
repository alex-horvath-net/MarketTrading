var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddDebug().AddConsole();

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Configure authentication & authorization
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options => {
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

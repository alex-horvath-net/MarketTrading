var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddDebug().AddConsole();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options => {
    options.Authority = "https://localhost:5002"; // Blazor & Gateway trust this
    options.Audience = "gateway";
    options.RequireHttpsMetadata = false;
});

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();
app.MapGet("/ping", () => "pong");
app.Run();

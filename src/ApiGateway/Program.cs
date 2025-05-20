var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddDebug().AddConsole();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options => {
    options.Authority = "https://yourbank_identityservice:443";  // Identity provider
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
app.Run();
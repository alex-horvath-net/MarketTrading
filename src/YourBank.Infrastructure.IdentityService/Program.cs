using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YourBank.Infrastructure.IdentityService.Data;
using YourBank.Infrastructure.IdentityService.Options;

var builder = WebApplication.CreateBuilder(args);

// Bind configuration sections using the Options Pattern.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<AzureAdOptions>(builder.Configuration.GetSection("AzureAd"));
builder.Services.Configure<FacebookOptions>(builder.Configuration.GetSection("Facebook"));

// Configure EF Core & Identity (production password policies applied).
var connectionString = builder.Configuration.GetConnectionString("IdentityDb")
    ?? throw new Exception("Missing IdentityDb connection string");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configure authentication using JWT, Azure AD, and Facebook.
builder.Services.AddAuthentication(options => {
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("YourBank", options => {
    var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ValidateIssuerSigningKey = true
    };
})
.AddOpenIdConnect("AzureAD", options => {
    var azureAdOptions = builder.Configuration.GetSection("AzureAd").Get<AzureAdOptions>();
    options.Authority = azureAdOptions.Instance + azureAdOptions.TenantId;
    options.ClientId = azureAdOptions.ClientId;
    options.ClientSecret = azureAdOptions.ClientSecret;
    options.ResponseType = "code";
    options.SaveTokens = true;
})
.AddFacebook("Facebook", options => {
    var fbOptions = builder.Configuration.GetSection("Facebook").Get<FacebookOptions>();
    options.AppId = fbOptions.AppId;
    options.AppSecret = fbOptions.AppSecret;
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Apply migrations automatically. In production, consider a controlled migration process.
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

// Local registration endpoint with full error checking.
app.MapPost("/local/register", handler: async (UserManager<IdentityUser> userManager, string email, string password) => {
    var user = new IdentityUser { UserName = email, Email = email };
    var result = await userManager.CreateAsync(user, password);
    if (!result.Succeeded) {
        return Results.BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
    }
    await userManager.AddToRoleAsync(user, "Trader");
    return Results.Ok(new { Message = "Registration successful." });
});

// Local login endpoint that issues a JWT.
app.MapPost("/local/login", async (UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JwtOptions> jwtOptionsAccessor, string email, string password) => {
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
        return Results.BadRequest("Invalid credentials.");
    var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
    if (!result.Succeeded)
        return Results.BadRequest("Invalid credentials.");

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
    };

    var jwtOptions = jwtOptionsAccessor.Value;
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: jwtOptions.Issuer,
        audience: jwtOptions.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);
    return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
});

// Federated token exchange endpoint (example with Azure AD).
app.MapPost("/token/exchange", async (UserManager<IdentityUser> userManager, HttpContext context, IOptions<JwtOptions> jwtOptionsAccessor) => {
    var authResult = await context.AuthenticateAsync("AzureAD");
    if (!authResult.Succeeded)
        return Results.Unauthorized();
    var externalUserId = authResult.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var externalEmail = authResult.Principal?.FindFirst(ClaimTypes.Email)?.Value;
    if (string.IsNullOrEmpty(externalUserId))
        return Results.BadRequest("External user identifier missing.");

    var user = await userManager.FindByNameAsync(externalUserId);
    if (user == null) {
        user = new IdentityUser { UserName = externalUserId, Email = externalEmail };
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
            return Results.BadRequest("Unable to create local user.");
        await userManager.AddToRoleAsync(user, "Trader");
    }

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
    };

    var jwtOptions = jwtOptionsAccessor.Value;
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: jwtOptions.Issuer,
        audience: jwtOptions.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);
    return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
});

app.Run();

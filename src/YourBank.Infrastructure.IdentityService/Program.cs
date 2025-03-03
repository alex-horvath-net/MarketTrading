// Handles user authentication and authorization.
// It integrates with Azure AD for internal users and manages local accounts for external stakeholders.
// Issues your own “YourBank” JWT tokens.
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var sqlServerConnectionString = builder.Configuration.GetConnectionString("IdentityDb");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlServerConnectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";


    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequiredLength = 6;

    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedPhoneNumber = true;

    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("YourBank", options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidIssuer = "https://identity.yourbank.com",
        ValidateAudience = true,
        ValidAudience = "YourBankMicroservices",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecureKeyForLocalTokens!123"))
    };
})

.AddOpenIdConnect("AzureAD", options => {
    // Replace these settings with your Azure AD app registration values
    options.Authority = "https://login.microsoftonline.com/<YOUR_TENANT_ID>/v2.0";
    options.ClientId = "<YOUR_AZURE_AD_CLIENT_ID>";
    options.ClientSecret = "<YOUR_AZURE_AD_CLIENT_SECRET>";
    options.ResponseType = "code";
    options.SaveTokens = true;
})

.AddFacebook("Facebook", options => {
    // Replace with your Facebook App settings
    options.AppId = "<YOUR_FACEBOOK_APP_ID>";
    options.AppSecret = "<YOUR_FACEBOOK_APP_SECRET>";
});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Middleware
app.UseAuthentication();
app.UseAuthorization();

// ==========================
// 5. Define Endpoints
// ==========================

// 5.1 Local Registration (for external users)
app.MapPost("/local/register", async (
    UserManager<IdentityUser> userManager,
    string email,
    string password) => {
        var existing = await userManager.FindByEmailAsync(email);
        if (existing != null)
            return Results.BadRequest("User already exists.");

        var newUser = new IdentityUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(newUser, password);
        if (!result.Succeeded)
            return Results.BadRequest(result.Errors);

        // Optionally, assign a role (e.g., Trader)
        await userManager.AddToRoleAsync(newUser, "Trader");

        return Results.Ok($"User {email} registered successfully.");
    });

// 5.2 Local Login (for external users)
app.MapPost("/local/login", async (
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    string email,
    string password) => {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return Results.BadRequest("Invalid credentials.");

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        if (!signInResult.Succeeded)
            return Results.BadRequest("Invalid credentials.");

        var token = await GenerateYourBankTokenAsync(user, userManager);
        return Results.Ok(new { Token = token });
    });

// 5.3 External Token Exchange Endpoint
// This endpoint accepts an external token (from Azure AD or Facebook) and returns a YourBank JWT.
// The caller must indicate the provider via a query parameter (?provider=AzureAD or ?provider=Facebook).
app.MapPost("/token/exchange", async (
    UserManager<IdentityUser> userManager,
    HttpContext context) => {
        var provider = context.Request.Query["provider"].ToString();
        if (string.IsNullOrEmpty(provider))
            return Results.BadRequest("Provider not specified. Use ?provider=AzureAD or ?provider=Facebook.");

        // Authenticate using the specified external provider
        var authResult = await context.AuthenticateAsync(provider);
        if (!authResult.Succeeded)
            return Results.Unauthorized();

        // Extract unique identifier and email from external claims
        var externalUserId = authResult.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var externalEmail = authResult.Principal?.FindFirst(ClaimTypes.Email)?.Value
                           ?? authResult.Principal?.FindFirst("preferred_username")?.Value;

        if (string.IsNullOrEmpty(externalUserId))
            return Results.BadRequest("External user identifier not found.");

        // Map external identity to a local user.
        // For simplicity, we use the externalUserId as the local username.
        var localUser = await userManager.FindByNameAsync(externalUserId);
        if (localUser == null) {
            localUser = new IdentityUser {
                UserName = externalUserId,
                Email = externalEmail
            };
            var createResult = await userManager.CreateAsync(localUser);
            if (!createResult.Succeeded)
                return Results.BadRequest("Unable to create local user for external identity.");

            // Optionally, assign a default role (e.g., Trader)
            await userManager.AddToRoleAsync(localUser, "Trader");
        }

        // Issue a YourBank JWT token for the local user
        var token = await GenerateYourBankTokenAsync(localUser, userManager);
        return Results.Ok(new { Token = token });
    })
// Note: You may remove the provider requirement from authorization here if needed.
.RequireAuthorization();

// Run the application
app.Run();

// ==========================
// 6. Helper Methods
// ==========================
async Task<string> GenerateYourBankTokenAsync(IdentityUser user, UserManager<IdentityUser> userManager) {
    // Get roles to embed in the token
    var roles = await userManager.GetRolesAsync(user);
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? "")
    };

    foreach (var role in roles) {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecureKeyForLocalTokens!123"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "https://identity.yourbank.com",
        audience: "YourBankMicroservices",
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1) EF + Identity
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 2) JWT setup
var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opts => {
    opts.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/auth/register", async (RegisterDto dto, UserManager<IdentityUser> um) => {
    var user = new IdentityUser(dto.Email) { Email = dto.Email };
    var result = await um.CreateAsync(user, dto.Password);
    return result.Succeeded ? Results.Ok() : Results.ValidationProblem(result.Errors.ToDictionary(e => e.Code, e => new[] { e.Description }));
});

app.MapPost("/auth/login", async (LoginDto dto, SignInManager<IdentityUser> sm, IConfiguration cfg) => {
    var user = await sm.UserManager.FindByEmailAsync(dto.Email);
    if (user is null)
        return Results.Unauthorized();
    var ok = await sm.CheckPasswordSignInAsync(user, dto.Password, false);
    if (!ok.Succeeded)
        return Results.Unauthorized();

    var claims = new[] {
    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
    new Claim(JwtRegisteredClaimNames.Email, user.Email),
    // … any roles or extra claims …
  };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: cfg["Jwt:Issuer"],
        audience: cfg["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: creds);
    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
});

app.Run();

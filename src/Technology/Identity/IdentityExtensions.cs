using System.Security.Claims;
using System.Text.Json;
//using Technology.BlazorWebApp.Components.Account.Pages;
//using Technology.BlazorWebApp.Components.Account.Pages.Manage;
using Technology.Data.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Technology.Data.Identity.Model;

namespace Technology.Identity;
public static class IdentityExtensions {

    public static IServiceCollection AddIdentityServices(this IServiceCollection services, ConfigurationManager configuration) {
        services.AddScoped<IdentityUserAccessor>();
        services.AddScoped<IdentityRedirectManager>();
        services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
        services
            .AddAuthentication(options => {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        var connectionString = configuration.GetConnectionString("Identity") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<IdentityDB>((sp, options) => options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();


        services
            .AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<IdentityDB>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddSingleton<IEmailSender<User>, IdentityEmailSender>();


        return services;
    }

    // These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints, string loginCallbackAction, string linkLoginCallbackAction) {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapAccountGropup()
            .MapPerformExternalLogintEndpoint(loginCallbackAction)
            .MapLogoutEndpoint();

        var manageGroup = accountGroup.MapManageGropup()
            .MapLinkExternalLoginEndpoint(linkLoginCallbackAction)
            .MapDownloadPersonalDataEndpoint(endpoints);

        return accountGroup;
    }

    private static RouteGroupBuilder MapAccountGropup(this IEndpointRouteBuilder endpoints) {
        var accountGroup = endpoints.MapGroup("/Account");
        return accountGroup;
    }

    private static RouteGroupBuilder MapPerformExternalLogintEndpoint(this RouteGroupBuilder accountGroup, string loginCallbackAction) {
        accountGroup.MapPost("/PerformExternalLogin", (
            HttpContext context,
            [FromServices] SignInManager<User> signInManager,
            [FromForm] string provider,
            [FromForm] string returnUrl) => {
                IEnumerable<KeyValuePair<string, StringValues>> query = [
                    new("ReturnUrl", returnUrl),
                    new("Action", loginCallbackAction)];
                //new("Action", ExternalLogin.LoginCallbackAction)];

                var redirectUrl = UriHelper.BuildRelative(
                    context.Request.PathBase,
                    "/Account/ExternalLogin",
                    QueryString.Create(query));

                var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return TypedResults.Challenge(properties, [provider]);
            });

        return accountGroup;
    }

    private static RouteGroupBuilder MapLogoutEndpoint(this RouteGroupBuilder accountGroup) {
        accountGroup.MapPost("/Logout", async (
            ClaimsPrincipal user,
            SignInManager<User> signInManager,
            [FromForm] string returnUrl) => {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });
        return accountGroup;
    }

    private static RouteGroupBuilder MapManageGropup(this RouteGroupBuilder accountGroup) {
        var manageGroup = accountGroup.MapGroup("/Manage").RequireAuthorization();
        return manageGroup;
    }

    private static RouteGroupBuilder MapLinkExternalLoginEndpoint(this RouteGroupBuilder manageGroup, string linkLoginCallbackAction) {
        manageGroup.MapPost("/LinkExternalLogin", async (
            HttpContext context,
            [FromServices] SignInManager<User> signInManager,
            [FromForm] string provider) => {
                // Clear the existing external cookie to ensure a clean login process
                await context.SignOutAsync(IdentityConstants.ExternalScheme);

                var redirectUrl = UriHelper.BuildRelative(
                    context.Request.PathBase,
                    "/Account/Manage/ExternalLogins",
                    QueryString.Create("Action", linkLoginCallbackAction));
                //QueryString.Create("Action", ExternalLogins.LinkLoginCallbackAction));

                var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, signInManager.UserManager.GetUserId(context.User));
                return TypedResults.Challenge(properties, [provider]);
            });

        return manageGroup;
    }

    private static RouteGroupBuilder MapDownloadPersonalDataEndpoint(this RouteGroupBuilder manageGroup, IEndpointRouteBuilder endpoints) {
        manageGroup.MapPost("/DownloadPersonalData", async (
            HttpContext context,
            [FromServices] UserManager<User> userManager,
            [FromServices] AuthenticationStateProvider authenticationStateProvider) => {
                var user = await userManager.GetUserAsync(context.User);
                if (user is null) {
                    return Results.NotFound($"Unable to load user with ID '{userManager.GetUserId(context.User)}'.");
                }

                var userId = await userManager.GetUserIdAsync(user);
                var loggerFactory = endpoints.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var downloadLogger = loggerFactory.CreateLogger("DownloadPersonalData");
                downloadLogger.LogInformation("User with ID '{UserId}' asked for their personal data.", userId);

                // Only include personal data for download
                var personalData = new Dictionary<string, string>();
                var personalDataProps = typeof(User).GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
                foreach (var p in personalDataProps) {
                    personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
                }

                var logins = await userManager.GetLoginsAsync(user);
                foreach (var l in logins) {
                    personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
                }

                personalData.Add("Authenticator Key", (await userManager.GetAuthenticatorKeyAsync(user))!);
                var fileBytes = JsonSerializer.SerializeToUtf8Bytes(personalData);

                context.Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
                return TypedResults.File(fileBytes, contentType: "application/json", fileDownloadName: "PersonalData.json");
            });

        return manageGroup;
    }
}
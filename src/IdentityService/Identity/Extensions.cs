using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Identity;
public static class Extensions {
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration config) {
        // Token Forwarding 
        services.AddHttpContextAccessor();
        services.AddTransient<AccessTokenHandlerForBlazorServer>();

        var authenticationBuilder = services.AddAuthentication(options => {
            // Default authentication flow: use cookies + challenge with OIDC
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; 
        });

        authenticationBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme); // cookie authentication

        authenticationBuilder.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options => { // token validation
            config.GetSection("OpenIdConnect").Bind(options);

            // Good defaults for banks / secure apps
            options.SaveTokens = true; // Makes access_token/id_token available via GetTokenAsync()
            options.GetClaimsFromUserInfoEndpoint = true;

            options.ResponseType = "code"; // Authorization Code flow (secure)
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");

            // Optional — map additional claims
            options.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
            options.ClaimActions.MapJsonKey("role", "role");

            // Optional — add logging for diagnostics
            options.Events = new OpenIdConnectEvents {
                OnRemoteFailure = context => {
                    context.HandleResponse();
                    context.Response.Redirect("/login-failed");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}
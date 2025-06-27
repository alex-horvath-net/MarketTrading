using Infrastructure.Adapters.Identity.Data.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.IdentityManager;

public class IdentityManager {
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<IdentityManager> _logger;

    public IdentityManager(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ILogger<IdentityManager> logger) {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SignInResult?> LocalLogIn(string userName, string password, bool isPersistent, bool lockoutOnFailure) {
        var result = await _signInManager.PasswordSignInAsync(
            userName,
            password,
            isPersistent,
            lockoutOnFailure);
        return result;
    }

    public async Task ExternaLogOut(HttpContext httpContext) {
        if (HttpMethods.IsGet(httpContext.Request.Method)) {
            // Clear the existing external cookie to ensure a clean login process
            await httpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }
    }

    public string GetUserName(HttpContext? httpContext) =>
        httpContext?.User?.Identity?.IsAuthenticated == true ? 
        httpContext?.User.Identity?.Name ?? "unknown" :
        "guest";
}

public static class ExpertExtensions {
    public static IServiceCollection AddIdentityManager(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<IdentityManager>();

}
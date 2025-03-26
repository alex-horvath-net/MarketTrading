using Business.Experts.Trader.FindTransactions;
using Infrastructure.Adapters.Identity.Data.Model;
using Infrastructure.Technology.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Business.Experts.IdentityManager;

public class Expert {
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<Expert> _logger;

    public Expert(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ILogger<Expert> logger) {
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
}

public static class ExpertExtensions {
    public static IServiceCollection AddIdentityManager(this IServiceCollection services, ConfigurationManager config) => services
        .AddScoped<Expert>();
     
}
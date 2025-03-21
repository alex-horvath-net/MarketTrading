using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Business.Experts.IdentityManager;

public class IdentityManager {
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityRedirectManager _identityRedirectManager;
    private readonly ILogger<IdentityManager> _logger;

    public IdentityManager(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IdentityRedirectManager identityRedirectManager,
        ILogger<IdentityManager> logger) {
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _identityRedirectManager = identityRedirectManager ?? throw new ArgumentNullException(nameof(identityRedirectManager));
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

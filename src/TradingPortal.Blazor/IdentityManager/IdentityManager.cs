using Microsoft.AspNetCore.Identity;
using TradingPortal.Blazor.Components.Account;
using TradingPortal.Blazor.Data;

namespace TradingPortal.Blazor.IdentityManager;
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


    public string GetUriWithQueryParameters(string uri, Dictionary<string, object?> newQueryParameters) {

        return _identityRedirectManager.GetUriWithQueryParameters(uri, newQueryParameters);
    }


    public async Task<SignInResult?> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure) {
        var result = await _signInManager.PasswordSignInAsync(
            userName,
            password,
            isPersistent,
            lockoutOnFailure);
        return result;
    }
}

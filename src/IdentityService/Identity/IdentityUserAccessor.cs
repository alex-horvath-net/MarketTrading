using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Infrastructure.Adapters.Identity.Data.Model;

namespace IdentityService.Identity;
public sealed class IdentityUserAccessor(
    UserManager<User> userManager,
    IdentityRedirectManager redirectManager) {
    public async Task<User> GetRequiredUserAsync(HttpContext context) {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null) {
            redirectManager.RedirectTo("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}

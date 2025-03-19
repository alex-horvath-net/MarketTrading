using Microsoft.AspNetCore.Identity;
using TradingPortal.Blazor.Data;

namespace TradingPortal.Blazor.Components.Account {
    internal sealed class IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager) {
        public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context) {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null) {
                redirectManager.RedirectTo("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}

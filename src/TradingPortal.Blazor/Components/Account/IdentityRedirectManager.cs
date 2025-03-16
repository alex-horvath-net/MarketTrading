using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace TradingPortal.Blazor.Components.Account {
    public class IdentityRedirectManager(NavigationManager navigationManager) {
        public const string StatusCookieName = "Identity.StatusMessage";

        private static readonly CookieBuilder StatusCookieBuilder = new() {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            IsEssential = true,
            MaxAge = TimeSpan.FromSeconds(5),
        };

        [DoesNotReturn]
        public void RedirectTo(string? uri) {
            var relativeUri = GetLocalRelativeUriorExternalAbsoluteUri(uri);

            // During static rendering, NavigateTo throws a NavigationException which is handled by the framework as a redirect.
            // So as long as this is called from a statically rendered Identity component, the InvalidOperationException is never thrown.
            navigationManager.NavigateTo(relativeUri);
            throw new InvalidOperationException($"{nameof(IdentityRedirectManager)} can only be used during static rendering.");
        }

        [DoesNotReturn]
        public void RedirectTo(string relativeUri, Dictionary<string, object?> queryParameters) {
            //string absoluteUri = GenerateAbsoluteUri(relativeUri, queryParameters);
            //RedirectTo(absoluteUri);
            var newRelateUri = GetUriWithQueryParameters(relativeUri, queryParameters);
            RedirectTo(newRelateUri);
        }

        [DoesNotReturn]
        public void RedirectToWithStatus(string uri, string message, HttpContext context) {
            context.Response.Cookies.Append(StatusCookieName, message, StatusCookieBuilder.Build(context));
            RedirectTo(uri);
        }

        private string CurrentPath => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

        [DoesNotReturn]
        public void RedirectToCurrentPage() => RedirectTo(CurrentPath);

        [DoesNotReturn]
        public void RedirectToCurrentPageWithStatus(string message, HttpContext context)
            => RedirectToWithStatus(CurrentPath, message, context);

        private string GetLocalRelativeUriorExternalAbsoluteUri(string? uri) {
            uri ??= "";
            var relatuveUri =
                Uri.IsWellFormedUriString(uri, UriKind.Relative) ?
                uri :
                navigationManager.ToBaseRelativePath(uri);

            return relatuveUri;
        }
         
        public string GetUriWithQueryParameters(string uri, Dictionary<string, object?> newQueryParameters) {

            return navigationManager.GetUriWithQueryParameters(uri, newQueryParameters);
        }
    }
}   
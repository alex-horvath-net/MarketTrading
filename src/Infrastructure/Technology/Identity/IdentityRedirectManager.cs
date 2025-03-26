using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Technology.Identity;
public class IdentityRedirectManager(NavigationManager navigationManager) {
    public const string IdentityStatusMessageCookieName = "Identity.StatusMessage";

    private static readonly CookieBuilder StatusCookieBuilder = new() {
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromSeconds(5),
    };

    [DoesNotReturn]
    public void RedirectTo() => RedirectTo(CurrentPath);

    [DoesNotReturn]
    public void RedirectTo(string? uri) {
        uri = ConvertUriToRelative(uri);

        // During static rendering, NavigateTo throws a NavigationException which is handled by the framework as a redirect.
        // So as long as this is called from a statically rendered Identity component, the InvalidOperationException is never thrown.
        navigationManager.NavigateTo(uri);
        throw new InvalidOperationException($"{nameof(IdentityRedirectManager)} can only be used during static rendering.");
    }

    [DoesNotReturn]
    public void RedirectTo(string uri, Dictionary<string, object?> queryParameters) {
        uri = AddQueryParametersToUri(uri, queryParameters);
        navigationManager.NavigateTo(uri);
        throw new InvalidOperationException($"{nameof(IdentityRedirectManager)} can only be used during static rendering.");
    }

    [DoesNotReturn]
    public void RedirectTo(string message, HttpContext context) => RedirectTo(CurrentPath, message, context);

    [DoesNotReturn]
    public void RedirectTo(string uri, string message, HttpContext context) {
        context.Response.Cookies.Append(IdentityStatusMessageCookieName, message, StatusCookieBuilder.Build(context));
        RedirectTo(uri);
    }

    private string CurrentPath => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

    public string ConvertUriToRelative(string? uri) {
        uri ??= "";
        var relativeUri =
            Uri.IsWellFormedUriString(uri, UriKind.Relative) ?
            uri :
            navigationManager.ToBaseRelativePath(uri);

        return relativeUri;
    }

    public string AddQueryParametersToUri(string uri, Dictionary<string, object?> newQueryParameters) {

        return navigationManager.GetUriWithQueryParameters(uri, newQueryParameters);
    }

    private bool IsStaticRendering() {
        // ICheck if the current URI is being rendered statically
        return navigationManager.Uri.Contains("_blazor");
    }
}
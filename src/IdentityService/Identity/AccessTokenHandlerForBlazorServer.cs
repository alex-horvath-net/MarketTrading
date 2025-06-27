using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Identity;

public class AccessTokenHandlerForBlazorServer : DelegatingHandler {
    private readonly IHttpContextAccessor _contextAccessor;

    public AccessTokenHandlerForBlazorServer(IHttpContextAccessor contextAccessor) {
        _contextAccessor = contextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        var context = _contextAccessor.HttpContext;
        if (context == null)
            return await base.SendAsync(request, cancellationToken);

        var accessToken = await context.GetTokenAsync("access_token");
        if (string.IsNullOrWhiteSpace(accessToken))
            return await base.SendAsync(request, cancellationToken);
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}

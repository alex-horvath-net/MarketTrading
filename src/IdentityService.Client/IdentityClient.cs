using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace IdentityService.Client;

public class IdentityClient : IIdentityClient {
    private readonly HttpClient _httpClient;
    private readonly ILogger<IdentityClient> _logger;

    public IdentityClient(HttpClient httpClient, ILogger<IdentityClient> logger) {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task LogoutAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Logging out...");
        var response = await _httpClient.PostAsync("/api/identity/logout", null, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<UserProfileResponse> GetProfileAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Fetching user profile...");
        var response = await _httpClient.GetAsync("/api/identity/profile", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserProfileResponse>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to get profile.");
    }
}
